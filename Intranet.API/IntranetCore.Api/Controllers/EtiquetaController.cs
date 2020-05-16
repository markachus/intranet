using AutoMapper;
using IntranetCore.Data.Models;
using IntranetCore.Data.Entities;
using IntranetCore.Data.Helpers;
using IntranetCore.Data.Repositories;
using IntranetCore.Data.ResourceParameters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using IntranetCore.Api.Attributes;
using Microsoft.Extensions.Caching.Redis;


namespace Intranet.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/tags")]
    public class EtiquetaController : ControllerBase
    {
        private readonly IEtiquetaRepository _repository;
        private readonly IMapper mapper;

        public EtiquetaController(IEtiquetaRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this.mapper = mapper;
        }

        [Marvin.Cache.Headers.HttpCacheExpiration(MaxAge = 100, CacheLocation = Marvin.Cache.Headers.CacheLocation.Public)]
        [Marvin.Cache.Headers.HttpCacheValidation(MustRevalidate = false)]
        /// <summary>
        /// Obtiene las etiquetas teniendo en cuenta las opciones de paginación, ordenación y búsqueda.
        /// </summary>
        /// <param name="tagsParameters">Información de paginación ordenación y búsqueda. 
        /// <seealso cref="EtiquetasResourceParameters"/></param>
        /// <returns>ActionResult de EtiquetaModel</returns>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [HttpGet(Name = "GetTags")]
        public async Task<ActionResult<EtiquetaModel>> GetAll([FromQuery] EtiquetasResourceParameters tagsParameters)
        {

            if (tagsParameters == null) tagsParameters = new EtiquetasResourceParameters();
            var results = await _repository.GetAllAsync(tagsParameters);

            var previousPageLink = results.HasPrevious ? CreateEtiquetasResourceUri(tagsParameters, ResourceTypeUri.PreviousPage) : null;
            var nextPageLink = results.HasNext ? CreateEtiquetasResourceUri(tagsParameters, ResourceTypeUri.NextPage) : null;

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


            return Ok(mapper.Map<IEnumerable<EtiquetaModel>>(results));
        }
        private string CreateEtiquetasResourceUri(EtiquetasResourceParameters entradasParams, ResourceTypeUri type)
        {

            switch (type)
            {
                case ResourceTypeUri.NextPage:
                    return Url.Link("GetTags",
                        new
                        {
                            PageNumber = entradasParams.PageNumber + 1,
                            PageSize = entradasParams.PageSize
                        });
                case ResourceTypeUri.PreviousPage:
                    return Url.Link("GetTags",
                        new
                        {
                            PageNumber = entradasParams.PageNumber - 1,
                            PageSize = entradasParams.PageSize
                        });
                default:
                    return Url.Link("GetPosts",
                    new
                    {
                        PageNumber = entradasParams.PageNumber,
                        PageSize = entradasParams.PageSize
                    });
            }
        }


        /// <summary>
        /// Obtiene una etiqueta por su nombre
        /// </summary>
        /// <param name="nombre">Nombre de la etiqueta. Se ignoran las mayúsculas.</param>
        /// <returns></returns>
        /// <response code="200">Devuelve la etiqueta</response>
        /// <response code="404">No se encontró ninguna etiqueta con el nombre proporcionado</response>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [HttpGet("{nombre}", Name = "GetTag")]
        public async Task<ActionResult<EtiquetaModel>> GetTag(string nombre)
        {
            var tag = await _repository.GetAsync(nombre);
            if (tag == null) return NotFound();

            return Ok(mapper.Map<EtiquetaModel>(tag));

        }

        /// <summary>
        /// Crea una nueva etiqueta
        /// </summary>
        /// <param name="model">Modelo para crear una etiqueta nueva</param>
        /// <returns>ActionResult de EtiquetaModel</returns>
        /// <response code="201">Nueva etiqueta</response>
        /// <response code="400">La etiqueta ya existe</response>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [Consumes("application/json",
            "application/vnd.intranet.etiquetaforcreation.v1+json")]
        [Produces("application/vnd.intranet.etiqueta.v1+json")]
        [RequestHeaderMatchesMediaType("Content-Type",
            "application/json",
            "application/vnd.intranet.etiquetaforcreation.v1+json")]
        [HttpPost]
        public async Task<ActionResult<EtiquetaForCreationModel>> Post(EtiquetaModel model)
        {
            var tag = await _repository.GetAsync(model.Nombre);
            if (tag != null) return BadRequest($"La etiqueta {model.Nombre} ya existe");

            var newTag = mapper.Map<Etiqueta>(model);

            newTag.FechaCreacion = DateTime.Now;
            newTag.UsuarioCreacion = "Admin";
            newTag.FechaUltimaModificacion = DateTime.Now;
            newTag.UsuarioModificacion = "Admin";

            _repository.Add(newTag);

            await _repository.SaveChangesAsync();
            var newModel = mapper.Map<EtiquetaModel>(newTag);
            return CreatedAtRoute("GetTag", new { nombre = model.Nombre }, newModel);

        }


        /// <summary>
        /// Modifica la etiqueta
        /// </summary>
        /// <param name="nombre">Nombre identificador de la etiqueta</param>
        /// <param name="model">Información a actualizar de la etiqueta. Solo el HexColor</param>
        /// <returns></returns>
        /// <response code="404">No se encontró etiqueta con este nombre</response>
        /// <response code="200">Etiqueta modificada</response>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [HttpPut("{nombre}")]
        public async Task<ActionResult<EtiquetaModel>> Put(string nombre, EtiquetaForUpdateModel model)
        {
            var tag = await _repository.GetAsync(nombre);
            if (tag == null) return NotFound();

            mapper.Map(model, tag);
            tag.FechaUltimaModificacion = DateTime.Now;
            tag.UsuarioModificacion = "Admin";

            await _repository.SaveChangesAsync();
            return Ok(mapper.Map<EtiquetaModel>(tag));
        }

        /// <summary>
        /// Elimina un etiqueta por nombre
        /// </summary>
        /// <param name="nombre">Nombre de le etiqueta a eliminar</param>
        /// <returns>IActionResult</returns>
        /// <reponse code="409">La etiqueta está en uso</reponse>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [HttpDelete("{nombre}")]
        public async Task<IActionResult> Delete(string nombre)
        {

            var tag = await _repository.GetAsync(nombre);
            if (tag == null) return NotFound();

            try
            {
                _repository.Delete(tag);
                await _repository.SaveChangesAsync();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict();
            }

        }
        /// <summary>
        /// Actualiza parcialmente una etiqueta 
        /// </summary>
        /// <param name="nombre">Nombre de la etiqueta</param>
        /// <param name="patchDocument">Conjunto de operacions a aplicar a la etiqueta</param>
        /// <returns></returns>
        /// <remarks>Ejemplo: esto actualiza el color de la etiqueta \
        /// PATCH /tags/nombre \
        /// [ \
        ///     {\
        ///         "op": "replace", \
        ///         "from": "/HexColor", \
        ///         "value": "#000000" \
        ///     } \
        /// ]
        /// </remarks>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [HttpPatch("{nombre}")]
        public async Task<IActionResult> UpdatePartialEtiqueta(
            string nombre, 
            JsonPatchDocument<EtiquetaForUpdateModel> patchDocument) {

            Etiqueta tagFromRepo = await _repository.GetAsync(nombre);
            if (tagFromRepo == null) { 
                return NotFound(); 
            }

            EtiquetaForUpdateModel tagToPatch = mapper.Map<EtiquetaForUpdateModel>(tagFromRepo);
            patchDocument.ApplyTo(tagToPatch);

            if (!TryValidateModel(tagToPatch)) return ValidationProblem();

            mapper.Map(tagToPatch, tagFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Opciones disponibles en este recurso
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [Microsoft.AspNetCore.Mvc.HttpOptions]
        public IActionResult GetEtiquetasOptions() {

            HttpContext.Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,DELETE");
            return Ok();
        }

    }
}
