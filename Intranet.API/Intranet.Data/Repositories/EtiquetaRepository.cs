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

        IntranetDbContext IEtiquetaRepository.Context { get { return _context; } }

        public void Add(Etiqueta tag)
        {
            _context.Etiquetas.Add(tag);
        }

        public void Delete(Etiqueta tag)
        {   
            var qry = from p in _context.Entradas
                      where p.Etiquetas.Select(e => e.Nombre.ToUpper()).Contains(tag.Nombre.ToUpper())
                      select p;

            if (qry.Count() > 0) throw new InvalidOperationException($"No se puede eliminar {tag} porque está en uso");

            _context.Etiquetas.Remove(tag);
        }

        public async Task<Etiqueta[]> GetAllAsync()
        {

            return await _context.Etiquetas.
                OrderByDescending(t => t.FechaCreacion).
                ToArrayAsync();
        }

        public async Task<Etiqueta[]> GetAllAsync(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery)) {
                return await GetAllAsync();
            }

            searchQuery = searchQuery.Trim();
            return await _context.Etiquetas.Where( t => t.Nombre.Contains(searchQuery)).
                OrderByDescending(t => t.FechaCreacion).
                ToArrayAsync();
        }

        public async Task<Etiqueta> GetAsync(string nombre)
        {
            var  tag = await _context.Etiquetas.
                Where( t => t.Nombre.ToUpper() == nombre.ToUpper()).
                SingleOrDefaultAsync();
            return tag;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
