using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intranet.Data.Repositories
{
    public interface IEntradaRepository
    {
        Task<bool> SaveChangesAsync();

        Task<List<Entrada>> GetAllAsync(bool incluyeEtiquetas = false);
        Task<Entrada> GetAsync(Guid EntradaId);

        void Add(Entrada entrada);

        void Update(Entrada entrada);
        
        void Delete(Entrada entrada);

        void AddEtiqueta(Etiqueta tag);
        void RemoveEtiqueta(Etiqueta tag);

    }
}
