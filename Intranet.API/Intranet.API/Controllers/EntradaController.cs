﻿using AutoMapper;
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

        public EntradaController(IEntradaRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [Route]
        public async Task<IHttpActionResult> GetAll(string search = "", int page = 1, string sort = DESC)
        {
            try
            {
                var results = await _repository.GetAllAsync(true);
                return Ok(_mapper.Map<EntradaModel[]>(results));

            }
            catch (Exception ex)
            {
                return InternalServerError();
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
                    foreach(EtiquetaModel tagModel in model.Etiquetas) {
                        Etiqueta tag = await tagRepo.GetAsync(tagModel.Nombre);
                        if (tag == null) return BadRequest($"La etiqueta {tagModel.Nombre} no existe");
                        if (entrada.Etiquetas.Select( t => t.Nombre.ToUpper()).Contains(tagModel.Nombre.ToUpper()))
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


    }
}