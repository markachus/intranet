using Intranet.App.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.App.Services
{
    interface IEtiquetaService
    {

        string LastMessage { get; }

        HttpStatusCode LastStatusCode { get;}

        bool LastRequestOk { get; }

        void Update(EtiquetaModel model);
        void Delete(string nombre);
        void Add(EtiquetaModel etiquetaModel);

        Task<IEnumerable<EtiquetaModel>> GetAll();

    }
}
