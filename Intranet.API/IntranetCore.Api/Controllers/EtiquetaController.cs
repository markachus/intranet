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

namespace Intranet.API.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class EtiquetaController : ControllerBase
    {
        private readonly IEtiquetaRepository repository;
        private readonly IMapper mapper;

        public EtiquetaController(IEtiquetaRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet(Name = "GetTags")]
        public async Task<ActionResult<EtiquetaModel>> GetAll([FromQuery] EtiquetasResourceParameters tagsParameters)
        {

            if (tagsParameters == null) tagsParameters = new EtiquetasResourceParameters();
            var results = await repository.GetAllAsync(tagsParameters);

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


        [HttpGet("{nombre}", Name ="GetTag")]
        public async Task<ActionResult<EtiquetaModel>> GetTag( string nombre)
        {
                var tag = await repository.GetAsync(nombre);
                if (tag == null) return NotFound();

                return Ok(mapper.Map<EtiquetaModel>(tag));
            
        }

        [HttpPost]
        public async Task<ActionResult<EtiquetaForCreationModel>> Post(EtiquetaModel model)
        {
            var tag = await repository.GetAsync(model.Nombre);
            if (tag != null) return BadRequest($"La etiqueta {model.Nombre} ya existe");

            var newTag = mapper.Map<Etiqueta>(model);

            newTag.FechaCreacion = DateTime.Now;
            newTag.UsuarioCreacion = "Admin";
            newTag.FechaUltimaModificacion = DateTime.Now;
            newTag.UsuarioModificacion = "Admin";

            repository.Add(newTag);

            await repository.SaveChangesAsync();
            var newModel = mapper.Map<EtiquetaModel>(newTag);
            return CreatedAtRoute("GetTag", new { nombre = model.Nombre }, newModel);

        }

        [HttpPut("{nombre}")]
        public async Task<ActionResult<EtiquetaModel>> Put(string nombre, EtiquetaForUpdateModel model)
        {
            var tag = await repository.GetAsync(nombre);
            if (tag == null) return NotFound();

            mapper.Map(model, tag);
            tag.FechaUltimaModificacion = DateTime.Now;
            tag.UsuarioModificacion = "Admin";

            await repository.SaveChangesAsync();
            return Ok(mapper.Map<EtiquetaModel>(tag));
        }

        [HttpDelete("{nombre}")]
        public async Task<IActionResult> Delete(string nombre)
        {

            var tag = await repository.GetAsync(nombre);
            if (tag == null) return NotFound();
                
            try
            {
                repository.Delete(tag);
                await repository.SaveChangesAsync();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict();
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpOptions]
        public IActionResult GetEtiquetasOptions() {

            HttpContext.Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,DELETE");
            return Ok();
        }

    }
}
