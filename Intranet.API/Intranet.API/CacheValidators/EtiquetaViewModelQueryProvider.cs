using CacheCow.Server;
using Intranet.API.Models;
using Intranet.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Intranet.Api.Extensions;

namespace Intranet.Api.CacheValidators
{
    public class EtiquetaViewModelQueryProvider : ITimedETagQueryProvider<EtiquetaModel>
    {
        private readonly IEtiquetaRepository _repository;

        public EtiquetaViewModelQueryProvider(IEtiquetaRepository repository)
        {
            this._repository = repository;
        }

        public void Dispose()
        {
            //nothing
        }

        public Task<TimedEntityTagHeaderValue> QueryAsync(HttpActionContext context)
        {
            string nombre = null;
            var routeData = context.RequestContext.RouteData;
            if (routeData.Values.ContainsKey("nombre"))
                nombre = (string)(routeData.Values["nombre"]);

            if (!string.IsNullOrEmpty(nombre)) // Get last modified of etiqueta
            {
                var _lastModified = _repository.GetLastModified(nombre);
                if (_lastModified != null)
                    return Task.FromResult(new TimedEntityTagHeaderValue(_lastModified.Value.ToETagString()));
                else
                    return Task.FromResult((TimedEntityTagHeaderValue)null);
            }
            {
                return Task.FromResult((TimedEntityTagHeaderValue)null);
            }
            //else // all cars
            //{
            //    return Task.FromResult(new TimedEntityTagHeaderValue(_repository.GetMaxLastModified().ToETagString(_repository.GetCount())));
            //}
        }
    }
}