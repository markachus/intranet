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
    public class EtiquetaController : ApiController
    {
        private readonly IEtiquetaRepository repository;

        public EtiquetaController(IEtiquetaRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(await repository.GetAllAsync());
        }
    }
}
