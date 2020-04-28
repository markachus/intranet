using Intranet.Data.Entities;
using Intranet.Data.Helpers;
using Intranet.Data.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intranet.Data.Repositories
{
    public interface IEtiquetaRepository
    {
        IntranetDbContext Context { get; }
        Task<bool> SaveChangesAsync();

        Task<PagedList<Etiqueta>> GetAllAsync(EtiquetasResourceParameters tagsParemeters);
        Task<Etiqueta> GetAsync(string nombre);

        void Add(Etiqueta tag);

        void Delete(Etiqueta tag);

        //DateTimeOffset? GetLastModified(string nombre);
    }
}
