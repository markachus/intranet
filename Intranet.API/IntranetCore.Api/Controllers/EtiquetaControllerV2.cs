using AutoMapper;
using IntranetCore.Data.Helpers;
using IntranetCore.Data.Models;
using IntranetCore.Data.Repositories;
using IntranetCore.Data.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetCore.Api.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/v{version:ApiVersion}tags")]
    public class EtiquetaControllerV2 : ControllerBase
    {
        private readonly IEtiquetaRepository _repository;
        private readonly IMapper mapper;

        public EtiquetaControllerV2(IEtiquetaRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// V2. Obtiene las etiquetas teniendo en cuenta las opciones de paginación, ordenación y búsqueda.
        /// </summary>
        /// <param name="tagsParameters">Información de paginación ordenación y búsqueda. 
        /// <seealso cref="EtiquetasResourceParameters"/></param>
        /// <returns>ActionResult de EtiquetaModel</returns>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [HttpGet(Name = "GetTags2")]
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

    }
}
