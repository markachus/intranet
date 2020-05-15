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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using IntranetCore.Api.Attributes;
using Microsoft.Net.Http.Headers;

namespace Intranet.API.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:ApiVersion}/posts")]
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


        /// <summary>
        /// Obtiene todas las entradas teniendo en cuenta las opciones de paginacion, ordenación y búsqueda.
        /// </summary>
        /// <param name="param">Opciones para ordenar, buscar y paginar las entradas</param>
        /// <returns></returns>
        /// <response code="200">Entradas/posts</response>
        [RequestHeaderMatchesMediaType("Accept", 
            "application/json",
            "application/vnd.intranet.entrada.v1+json")]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [Produces("application/vnd.intranet.entrada.v1+json")]
        [HttpGet(Name = "GetPosts")]
        public async Task<ActionResult<EntradaModel>> GetAll([FromQuery] EntradasResourceParameters param)
        {
            if (param == null) param = new EntradasResourceParameters();

            if (!_propertyMappingService.IsPropertyMappingValid<EntradaModel, Entrada>(param.OrderBy))
            {
                return BadRequest();
            }

            var results = await _repository.GetAllAsync(param, true);

            var previousPageLink = results.HasPrevious ? CreateEntradasResourceUri(param, ResourceTypeUri.PreviousPage) : null;
            var nextPageLink = results.HasNext ? CreateEntradasResourceUri(param, ResourceTypeUri.NextPage) : null;

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

        private string CreateEntradasResourceUri(EntradasResourceParameters entradasParams, ResourceTypeUri type)
        {

            switch (type)
            {
                case ResourceTypeUri.NextPage:

                    return Url.Link("GetPosts",
                        new
                        {
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

        /// <summary>
        /// Obtiene una entrada por su Id
        /// </summary>
        /// <param name="entradaId">Id de la entrada</param>
        /// <returns></returns>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [HttpGet("{entradaId}", Name = "GetEntrada")]
        public async Task<ActionResult<EntradaModel>> Get(Guid entradaId)
        {

            var entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound();

            EntradaModel model = _mapper.Map<EntradaModel>(entrada);
            return Ok(model);
        }

        /// <summary>
        /// Crea una nueva entrada y asocia las etiquetas con ésta
        /// </summary>
        /// <param name="model">Información para crear una nueva entrada</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(EntradaForCreationModel model)
        {
            Entrada entrada = _mapper.Map<Entrada>(model);
            entrada.UsuarioCreacion = "Admin";
            entrada.FechaCreacion = DateTime.Now;
            entrada.UsuarioModificacion = "Admin";
            entrada.FechaUltimaModificacion = DateTime.Now;
            _repository.Add(entrada);

            //Creamos la asociación con las etiquetas
            EtiquetaRepository tagRepo = new EtiquetaRepository(this._repository.Context);
            foreach (String tagModel in model.Etiquetas)
            {

                Etiqueta tag = await tagRepo.GetAsync(tagModel);

                if (tag == null) return BadRequest($"La etiqueta {tagModel} no existe");

                else
                {
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

        /// <summary>
        /// Modifica una entrada por su identificador
        /// </summary>
        /// <param name="entradaId">Identificador de la entrada</param>
        /// <param name="model">Información para modificar una entrada</param>
        /// <response code="200">Entrada modificada</response>
        /// <response code="404">No se encontró la entrada con ese identificador</response>
        /// <returns></returns>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        [Consumes("application/json")]
        [HttpPut("{entradaId}")]
        public async Task<ActionResult<EntradaModel>> Put(Guid entradaId, EntradaModel model)
        {
            Entrada entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound();

            _mapper.Map(model, entrada);
            await _repository.SaveChangesAsync();
            return Ok(_mapper.Map<EntradaModel>(entrada));
        }


        /// <summary>
        /// Elimina una entrada y las associaciones con etiquetas que tuviese
        /// </summary>
        /// <param name="entradaId">identfiador de la entrada</param>
        /// <returns></returns>
        /// <response code="204">Entrada eliminada</response>
        /// <response code="404">No se encontró la entrada con ese identificador</response>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [HttpDelete("{entradaId}")]
        public async Task<IActionResult> Delete(Guid entradaId)
        {
            Entrada entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound();
            _repository.Delete(entrada);

            await _repository.SaveChangesAsync();
            return NoContent();

        }

        /// <summary>
        /// Obtiene una etiqueta asociada a una entrada
        /// </summary>
        /// <param name="entradaId">Identificador de la entrada</param>
        /// <param name="tagNombre">Nombre de la etiqueta</param>
        /// <response code="200">Entrada solicitada</response>
        /// <response code="404">Entrada no encontrada o etiqueta no associada a entrada</response>
        /// <returns></returns>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [HttpGet("{entradaId}/tags/{tagNombre}", Name = "GetEtiqueta")]
        public async Task<ActionResult<EtiquetaModel>> GetEtiqueta(Guid entradaId, string tagNombre)
        {
            Entrada entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound();

            Etiqueta tag = entrada.Etiquetas
                .Where(t => t.Etiqueta.Nombre.ToUpper() == tagNombre.ToUpper())
                .Select(t => t.Etiqueta)
                .SingleOrDefault();

            if (tag == null) return NotFound();

            return Ok(_mapper.Map<EtiquetaModel>(tag));
        }

        /// <summary>
        /// Da de alta una associación entre una entrada y una etiqueta si no existía ya previamente
        /// </summary>
        /// <param name="entradaId">Identificador de la entrada</param>
        /// <param name="model">Información para associar a una etiqueta</param>
        /// <returns></returns>
        /// <response code="201">Se ha associado la etiqueta a la entrada si no lo estaba ya</response>
        /// <response code="404">Entrada o etiquetas no encontradas</response>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        [Consumes("application/json")]
        [HttpPost("{entradaId}/tags")]
        public async Task<ActionResult<EtiquetaModel>> AddEtiqueta(Guid entradaId, EtiquetaForAssociationModel model)
        {
            EtiquetaRepository tagRepo = new EtiquetaRepository(_repository.Context);


            Entrada entrada = await _repository.GetAsync(entradaId);
            if (entrada == null) return NotFound(); //Entrada no encontrada

            Etiqueta tag = await tagRepo.GetAsync(model.Nombre);
            if (tag == null) return NotFound(); //Etiqueta no encontrada

            if (!entrada.Etiquetas.Select(t => t.Etiqueta.Nombre.ToUpper()).Contains(tag.Nombre.ToUpper()))
            {
                _repository.AddEtiqueta(entrada, tag);

                await _repository.SaveChangesAsync();

                return CreatedAtRoute(
                    "GetEtiqueta",
                    new { entradaId, tagNombre = model.Nombre },
                    _mapper.Map<EtiquetaModel>(tag));

            }
            else
            {
                return BadRequest($"Etiqueta {model.Nombre} usada");
            }
        }

        /// <summary>
        /// Elimina la associacón entre una entrada y una etiqueta
        /// </summary>
        /// <param name="entradaId">Id de la entrada</param>
        /// <param name="tagNombre">Nombre de la etiqueta</param>
        /// <returns></returns>
        /// <response code="204">Eliminada associación de etiqueta y entrada si existía</response>
        /// <response code="404">Entrada o etiquetas no encontradas</response>
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
                .Contains(tag.Nombre.ToUpper()))
            {
                _repository.RemoveEtiqueta(entrada, tag);

                await _repository.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
