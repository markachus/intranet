using AutoMapper;
using Intranet.API.Models;
using Intranet.Data.Entities;
using Intranet.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Intranet.API.Controllers
{
    [RoutePrefix("api/tags")]
    public class EtiquetaController : ApiController
    {
        private readonly IEtiquetaRepository repository;
        private readonly IMapper mapper;

        public EtiquetaController(IEtiquetaRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [Route]
        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(mapper.Map<EtiquetaModel[]>(await repository.GetAllAsync()));
        }

        [Route("{nombre}", Name ="GetTag")]
        public async Task<IHttpActionResult> Get(string nombre)
        {
            try
            {
                var tag = await repository.GetAsync(nombre);
                if (tag == null) return NotFound();

                return Ok(mapper.Map<EtiquetaModel>(tag));
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
            
        }

        [Route()]
        public async Task<IHttpActionResult> Post(EtiquetaModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var tag = await repository.GetAsync(model.Nombre);
                    if (tag != null) return BadRequest($"La etiqueta {model.Nombre} ya existe");

                    var newTag = mapper.Map<Etiqueta>(model);

                    newTag.FechaCreacion = DateTime.Today;
                    newTag.UsuarioCreacion = "Admin";

                    repository.Add(newTag);
                    if (await repository.SaveChangesAsync())
                    {

                        var newModel = mapper.Map<EtiquetaModel>(newTag);

                        return CreatedAtRoute("GetTag", new { nombre = model.Nombre }, newModel);
                    }
                    else
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

        [Route()]
        public async Task<IHttpActionResult> Put(EtiquetaModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var tag = await repository.GetAsync(model.Nombre);
                    if (tag != null) return BadRequest($"La etiqueta {model.Nombre} ya existe");

                    var newTag = mapper.Map<Etiqueta>(model);

                    newTag.FechaCreacion = DateTime.Today;
                    newTag.UsuarioCreacion = "Admin";

                    repository.Add(newTag);
                    if (await repository.SaveChangesAsync())
                    {

                        var newModel = mapper.Map<EtiquetaModel>(newTag);

                        return CreatedAtRoute("GetTag", new { nombre = model.Nombre }, newModel);
                    }
                    else
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

        [Route("{nombre}")]
        public async Task<IHttpActionResult> Put(string nombre, EtiquetaModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tag = await repository.GetAsync(nombre);
                    if (tag == null) return NotFound();

                    mapper.Map(model, tag);
                    tag.FechaUltimaModificacion = DateTime.Now;
                    tag.UsuarioModificacion = "Admin";

                    if (await repository.SaveChangesAsync()) {
                        return Ok(mapper.Map<EtiquetaModel>(tag));
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

        public async Task<IHttpActionResult> Delete(string nombre)
        {
            try
            {
                var tag = await repository.GetAsync(nombre);
                if (tag == null) return NotFound();
                
                try
                {
                    repository.Delete(tag);
                }
                catch (InvalidOperationException ex)
                {
                    return Conflict();
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

    }
}
