using AutoMapper;
using Intranet.API.Data;
using Intranet.Data.Models;
using Intranet.Data.Entities;
using Intranet.Data.Helpers;
using Intranet.Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Intranet.API.Controllers
{
    [RoutePrefix("api/posts")]
    public class EntradaController : ApiController
    {
        private const string ASC = "ASC";
        private const string DESC = "DESC";
        private readonly IEntradaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public EntradaController(IEntradaRepository repository, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._propertyMappingService = propertyMappingService;
        }

        [Route(Name = "GetPosts")]
        public async Task<IHttpActionResult> GetAll([FromUri] EntradasResourceParameters param)
        {
            try
            {
                if (param == null) param = new EntradasResourceParameters();

                if (!_propertyMappingService.IsPropertyMappingValid<EntradaModel, Entrada>(param.OrderBy)){
                    return BadRequest();
                }

                var results = await _repository.GetAllAsync(param, true);

                var previousPageLink = results.HasPrevious ? CreateEntradasResourceUri(param, ResourceTypeUri.PreviousPage) : null;
                var nextPageLink = results.HasNext? CreateEntradasResourceUri(param, ResourceTypeUri.NextPage) : null;

                var paginationMetadata = new
                {
                    totalCount = results.TotalCount,
                    pageSize = results.PageSize,
                    currentPage = results.CurrentPage,
                    totalPages = results.TotalPages,
                    previousPageLink,
                    nextPageLink
                };

                System.Web.HttpContext.Current.Response.Headers.
                    Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                return Ok(_mapper.Map<EntradaModel[]>(results));
                
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private string CreateEntradasResourceUri(EntradasResourceParameters entradasParams, ResourceTypeUri type) {

            switch (type) {
                case ResourceTypeUri.NextPage:

                    return Url.Link("GetPosts", 
                        new { 
                            PageNumber = entradasParams.PageNumber + 1,
                            entradasParams.PageSize,
                            entradasParams.OrderBy,
                            entradasParams.SearchQuery
                        });

                case ResourceTypeUri.PreviousPage:

                    return Url.Link("GetPosts", new
                    {
                        PageNumber = entradasParams.PageNumber - 1,
                        entradasParams.PageSize,
                        entradasParams.OrderBy,
                        entradasParams.SearchQuery
                    });
                default:

                    return Url.Link("GetPosts", new
                    {
                        PageNumber = entradasParams.PageNumber,
                        entradasParams.PageSize,
                        entradasParams.OrderBy,
                        entradasParams.SearchQuery
                    });
            }
        }

        [Route("{entradaId}", Name = "GetEntrada")]
        public async Task<IHttpActionResult> Get(Guid entradaId) {
            try
            {
                var entrada = await _repository.GetAsync(entradaId);
                if (entrada == null) return NotFound();

                EntradaModel model = _mapper.Map<EntradaModel>(entrada);
                return Ok(model);
            }
            catch (Exception ex)
            {

                return InternalServerError();
            }
        }

        [Route]
        public async Task<IHttpActionResult> Post(EntradaModel model) {
            try
            {
                if (ModelState.IsValid)
                {
                    Entrada entrada = _mapper.Map<Entrada>(model);
                    entrada.UsuarioCreacion = "Admin";
                    entrada.FechaCreacion = DateTime.Now;
                    _repository.Add(entrada);

                    //Creamos la asociación con las etiquetas
                    EtiquetaRepository tagRepo = new EtiquetaRepository(this._repository.Context);
                    foreach (EtiquetaModel tagModel in model.Etiquetas) {
                        Etiqueta tag = await tagRepo.GetAsync(tagModel.Nombre);
                        if (tag == null) return BadRequest($"La etiqueta {tagModel.Nombre} no existe");
                        if (entrada.Etiquetas.Select(t => t.Nombre.ToUpper()).Contains(tagModel.Nombre.ToUpper()))
                        {
                            return BadRequest($"Etiqueta {tagModel.Nombre} duplicada");
                        }
                        entrada.Etiquetas.Add(tag);
                    }

                    if (await _repository.SaveChangesAsync())
                    {
                        EntradaModel newModel = _mapper.Map<EntradaModel>(entrada);
                        return CreatedAtRoute("GetEntrada", new { entradaId = entrada.Id }, newModel);

                    } else
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest(ModelState);
        }

        [Route("{entradaId}")]
        public async Task<IHttpActionResult> Put(Guid entradaId, EntradaModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Entrada entrada = await _repository.GetAsync(entradaId);
                    if (entrada == null) return NotFound();

                    _mapper.Map(model, entrada);
                    if (await _repository.SaveChangesAsync())
                    {
                        return Ok(_mapper.Map<EntradaModel>(entrada));
                    }
                    else {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest(ModelState);
        }


        [Route("{entradaId}")]
        public async Task<IHttpActionResult> Delete(Guid entradaId)
        {
            try
            {
                Entrada entrada = await _repository.GetAsync(entradaId);
                if (entrada == null) return NotFound();
                _repository.Delete(entrada);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        
        [Route("{entradaId}/tags/{tagNombre}", Name = "GetEtiqueta")]
        public async Task<IHttpActionResult> GetEtiqueta(Guid entradaId, string tagNombre) {
            try
            {
                Entrada entrada = await _repository.GetAsync(entradaId);
                if (entrada == null) return NotFound();

                Etiqueta tag = entrada.Etiquetas.Where(t => t.Nombre.ToUpper() == tagNombre.ToUpper()).SingleOrDefault();
                if (tag == null) return NotFound();

                return Ok(_mapper.Map<EtiquetaModel>(tag));
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        
        }


        [HttpPost]
        [Route("{entradaId}/tags")]
        public async Task<IHttpActionResult> AddEtiqueta(Guid entradaId, EtiquetaModel model)
        {
            try
            {
                EtiquetaRepository tagRepo = new EtiquetaRepository(_repository.Context);

                if (ModelState.IsValid) {
                    Entrada entrada = await _repository.GetAsync(entradaId);
                    if (entrada == null) return NotFound(); //Entrada no encontrada

                    Etiqueta tag = await tagRepo.GetAsync(model.Nombre);
                    if (tag == null) return NotFound(); //Etiqueta no encontrada

                    if (!entrada.Etiquetas.Select( t => t.Nombre.ToUpper()).Contains(tag.Nombre.ToUpper())) {
                        _repository.AddEtiqueta(entrada, tag);

                        if (await _repository.SaveChangesAsync())
                        {
                            return CreatedAtRoute(
                                "GetEtiqueta", 
                                new { entradaId, tagNombre = model.Nombre}, 
                                _mapper.Map<EtiquetaModel>(model));
                        }
                        else {
                            return InternalServerError();
                        }

                    } else
                    {
                        return BadRequest($"Etiqueta {model.Nombre} usada");
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("{entradaId}/tags/{tagNombre}")]
        public async Task<IHttpActionResult> DeleteEtiqueta(Guid entradaId, string tagNombre)
        {
            try
            {
                EtiquetaRepository tagRepo = new EtiquetaRepository(_repository.Context);

                    Entrada entrada = await _repository.GetAsync(entradaId);
                    if (entrada == null) return NotFound(); //Entrada no encontrada

                    Etiqueta tag = await tagRepo.GetAsync(tagNombre);
                    if (tag == null) return NotFound(); //Etiqueta no encontrada

                    if (entrada.Etiquetas.Select(t => t.Nombre.ToUpper()).Contains(tag.Nombre.ToUpper()))
                    {
                        _repository.RemoveEtiqueta(entrada, tag);

                        if (await _repository.SaveChangesAsync())
                        {
                        return Ok();
                        }
                        else
                        {
                            return InternalServerError();
                        }
                }
                else
                {
                    return Ok(); //Dudo qué codigo devolver si no se ha hecho nada
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }



    }
}
