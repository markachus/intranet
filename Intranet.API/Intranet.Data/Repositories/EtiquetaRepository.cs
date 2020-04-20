using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Data.Repositories
{
    public class EtiquetaRepository : IEtiquetaRepository
    {
        private readonly IntranetDbContext _context;

        public EtiquetaRepository(IntranetDbContext context)
        {
            this._context = context;
        }

        public void Add(Etiqueta tag)
        {
            _context.Etiquetas.Add(tag);
        }

        public void Delete(Etiqueta tag)
        {
            _context.Etiquetas.Remove(tag);
        }

        public async Task<Etiqueta[]> GetAllAsync()
        {

            return await _context.Etiquetas.
                OrderByDescending(t => t.FechaCreacion).
                ToArrayAsync();
        }

        public async Task<Etiqueta> GetAsync(string nombre)
        {
            var  tag = await _context.Etiquetas.FindAsync(nombre);
            return tag;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
