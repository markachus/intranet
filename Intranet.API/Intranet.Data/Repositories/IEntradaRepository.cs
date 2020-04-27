using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intranet.Data.Repositories
{
    public interface IEntradaRepository
    {
        Task<bool> SaveChangesAsync();

        IntranetDbContext Context { get; }

        Task<Entrada[]> GetAllAsync(bool incluyeEtiquetas = false);
        Task<Entrada> GetAsync(Guid EntradaId);

        void Add(Entrada entrada);
        
        void Delete(Entrada entrada);

        void AddEtiqueta(Entrada entrada, Etiqueta tag);
        void RemoveEtiqueta(Entrada entrada, Etiqueta tag);


    }
}
