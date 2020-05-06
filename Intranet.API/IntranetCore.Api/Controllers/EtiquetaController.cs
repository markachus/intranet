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

namespace Intranet.API.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class EtiquetaController : ControllerBase
    {
        private readonly IEtiquetaRepository _repository;
        private readonly IMapper mapper;

        public EtiquetaController(IEtiquetaRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this.mapper = mapper;
        }

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


        [HttpGet("{nombre}", Name = "GetTag")]
        public async Task<ActionResult<EtiquetaModel>> GetTag(string nombre)
        {
            var tag = await _repository.GetAsync(nombre);
            if (tag == null) return NotFound();

            return Ok(mapper.Map<EtiquetaModel>(tag));

        }

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


        [Microsoft.AspNetCore.Mvc.HttpOptions]
        public IActionResult GetEtiquetasOptions() {

            HttpContext.Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,DELETE");
            return Ok();
        }

    }
}
