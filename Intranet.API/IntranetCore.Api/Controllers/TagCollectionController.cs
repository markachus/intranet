using AutoMapper;
using IntranetCore.Api.Helper;
using IntranetCore.Data.Entities;
using IntranetCore.Data.Models;
using IntranetCore.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetCore.Api.Controllers
{
    [Produces("application/json", "application/xml")]
    [ApiVersion("2.0")]
    [Route("api/v{version:ApiVersion}/tagscollection")]
    [ApiController]
    public class TagCollectionController : ControllerBase
    {
        private readonly IEtiquetaRepository _repository;
        private readonly IMapper _mapper;

        public TagCollectionController(IEtiquetaRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        /// <summary>
        /// List a set of tags specified by their names
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <reponse code="200">List of tags specified</reponse>
        /// <reponse code="404">One of the tags does not exist</reponse>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [HttpGet("({ids})", Name = "GetTagCollection")]
        public async Task<ActionResult<IEnumerable<EtiquetaModel>>>
            GetTagCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<string> ids) {

            if (ids == null) return BadRequest();

            List<Etiqueta> tagsFromRepo = new List<Etiqueta>();

            foreach (string tagId in ids) {
                var tagFromRepo = await _repository.GetAsync(tagId);
                if (tagFromRepo == null) return NotFound();
                tagsFromRepo.Add(tagFromRepo);
            }

            return Ok(_mapper.Map<IEnumerable<EtiquetaModel>>(tagsFromRepo));

        }


        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<EtiquetaModel>>> CreateTagCollection(IEnumerable<EtiquetaForCreationModel> tags)
        {

            var tagEntities = this._mapper.Map<IEnumerable<Etiqueta>>(tags);

            foreach (Etiqueta eti in tagEntities) {
                _repository.Add(eti);
            }

            await _repository.SaveChangesAsync();

            var tagsToReturn = _mapper.Map<IEnumerable<EtiquetaModel>>(tagEntities);
            var idsAsString = string.Join(", ", tagsToReturn.Select(t => t.Nombre));

            return CreatedAtRoute(
                "GetTagCollection", 
                new { ids = idsAsString }, 
                tagsToReturn);
        }

    }
}
