using IntranetCore.Data.Entities;
using IntranetCore.Data.Helpers;
using IntranetCore.Data.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntranetCore.Data.Repositories
{
    public interface IEtiquetaRepository
    {
        IntranetDbContext Context { get; }
        Task<bool> SaveChangesAsync();

        Task<PagedList<Etiqueta>> GetAllAsync(EtiquetasResourceParameters tagsParemeters);
        Task<Etiqueta> GetAsync(string nombre);

        void Add(Etiqueta tag);

        void Delete(Etiqueta tag);
    }
}
