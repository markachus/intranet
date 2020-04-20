using Intranet.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intranet.Data.Repositories
{
    public interface IEtiquetaRepository
    {
        Task<bool> SaveChangesAsync();

        Task<Etiqueta[]> GetAllAsync();
        Task<Etiqueta> GetAsync(string nombre);

        void Add(Etiqueta tag);

        void Delete(Etiqueta tag);

    }
}
