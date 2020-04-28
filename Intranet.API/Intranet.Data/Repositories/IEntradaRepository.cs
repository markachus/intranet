using Intranet.API.Data;
using Intranet.Data.Entities;
using Intranet.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intranet.Data.Repositories
{
    public interface IEntradaRepository
    {
        Task<bool> SaveChangesAsync();

        IntranetDbContext Context { get; }

        Task<PagedList<Entrada>> GetAllAsync(EntradasResourceParameters param, bool incluyeEtiquetas = false);
        Task<Entrada> GetAsync(Guid EntradaId);

        void Add(Entrada entrada);
        
        void Delete(Entrada entrada);

        void AddEtiqueta(Entrada entrada, Etiqueta tag);
        void RemoveEtiqueta(Entrada entrada, Etiqueta tag);


    }
}
