using AutoMapper;
using IntranetCore.API.Data;
using IntranetCore.Data.Models;
using IntranetCore.Data.Entities;
using IntranetCore.Data.Helpers;
using IntranetCore.Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Intranet.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class EntradaController : ControllerBase
    {
        private readonly IEntradaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public EntradaController(IEntradaRepository repository, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._propertyMappingService = propertyMappingService;
        }

        [HttpGet(Name = "GetPosts")]
        public async Task<ActionResult<EntradaModel>> GetAll([FromQuery] EntradasResourceParameters param)
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

                HttpContext.Response.Headers.
                    Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                return Ok(_mapper.Map<EntradaModel[]>(results));
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

        [HttpGet("{entradaId}", Name = "GetEntrada")]
        public async Task<ActionResult<EntradaModel>> Get(Guid entradaId) {

            var entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound();

            EntradaModel model = _mapper.Map<EntradaModel>(entrada);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EntradaForCreationModel model) {
            Entrada entrada = _mapper.Map<Entrada>(model);
            entrada.UsuarioCreacion = "Admin";
            entrada.FechaCreacion = DateTime.Now;
            entrada.UsuarioModificacion = "Admin";
            entrada.FechaUltimaModificacion = DateTime.Now;
            _repository.Add(entrada);

            //Creamos la asociación con las etiquetas
            EtiquetaRepository tagRepo = new EtiquetaRepository(this._repository.Context);
            foreach (String tagModel in model.Etiquetas) {
                
                Etiqueta tag = await tagRepo.GetAsync(tagModel);

                if (tag == null) return BadRequest($"La etiqueta {tagModel} no existe");

                else {
                    //Create assocation
                    if (entrada.Etiquetas.Select(t => t.Etiqueta.Nombre.ToUpper()).Contains(tagModel.ToUpper()))
                    {
                        return BadRequest($"Etiqueta {tagModel} duplicada");
                    }

                    entrada.Etiquetas.Add(new EntradaEtiqueta { Entrada = entrada, Etiqueta = tag });
                }
                
            }

            await _repository.SaveChangesAsync();
            EntradaModel newModel = _mapper.Map<EntradaModel>(entrada);
            return CreatedAtRoute("GetEntrada", new { entradaId = entrada.Id }, newModel);
        }

        [HttpPut("{entradaId}")]
        public async Task<ActionResult<EntradaModel>> Put(Guid entradaId, EntradaModel model)
        {
            Entrada entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound();

            _mapper.Map(model, entrada);
            await _repository.SaveChangesAsync();
            return Ok(_mapper.Map<EntradaModel>(entrada));
        }


        [HttpDelete("{entradaId}")]
        public async Task<IActionResult> Delete(Guid entradaId)
        {
                Entrada entrada = await _repository.GetAsync(entradaId);
                if (entrada == null) return NotFound();
                _repository.Delete(entrada);

            await _repository.SaveChangesAsync();
            return NoContent();
                
        }

        
        [HttpGet("{entradaId}/tags/{tagNombre}", Name = "GetEtiqueta")]
        public async Task<ActionResult<EtiquetaModel>> GetEtiqueta(Guid entradaId, string tagNombre) {
            Entrada entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound();

            Etiqueta tag = entrada.Etiquetas
                .Where(t => t.Etiqueta.Nombre.ToUpper() == tagNombre.ToUpper())
                .Select(t => t.Etiqueta)
                .SingleOrDefault();

            if (tag == null) return NotFound();

            return Ok(_mapper.Map<EtiquetaModel>(tag));
        }


        [HttpPost("{entradaId}/tags")]
        public async Task<ActionResult<EtiquetaModel>> AddEtiqueta(Guid entradaId, EtiquetaForAssociationModel model)
        {
                EtiquetaRepository tagRepo = new EtiquetaRepository(_repository.Context);


                Entrada entrada = await _repository.GetAsync(entradaId);
                if (entrada == null) return NotFound(); //Entrada no encontrada

                Etiqueta tag = await tagRepo.GetAsync(model.Nombre);
                if (tag == null) return NotFound(); //Etiqueta no encontrada

                if (!entrada.Etiquetas.Select( t => t.Etiqueta.Nombre.ToUpper()).Contains(tag.Nombre.ToUpper())) {
                    _repository.AddEtiqueta(entrada, tag);

                    await _repository.SaveChangesAsync();
                        
                    return CreatedAtRoute(
                        "GetEtiqueta", 
                        new { entradaId, tagNombre = model.Nombre }, 
                        _mapper.Map<EtiquetaModel>(tag));

                } else
                {
                    return BadRequest($"Etiqueta {model.Nombre} usada");
                }
        }

        [HttpDelete("{entradaId}/tags/{tagNombre}")]
        public async Task<IActionResult> DeleteEtiqueta(Guid entradaId, string tagNombre)
        {
            EtiquetaRepository tagRepo = new EtiquetaRepository(_repository.Context);

            Entrada entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound(); //Entrada no encontrada

            Etiqueta tag = await tagRepo.GetAsync(tagNombre);
            if (tag == null) return NotFound(); //Etiqueta no encontrada

            if (entrada.Etiquetas
                .Select(t => t.Etiqueta.Nombre.ToUpper())
                .Contains(tag.Nombre.ToUpper())) {
                _repository.RemoveEtiqueta(entrada, tag);

            await _repository.SaveChangesAsync();
                return NoContent();
            } else
            {
                return NoContent(); //Dudo qué codigo devolver si no se ha hecho nada
            }
        }
    }
}
