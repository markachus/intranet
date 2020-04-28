using Intranet.Data.Entities;
using Intranet.Data.Helpers;
using Intranet.Data.ResourceParameters;
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

        //public async Task<Etiqueta[]> GetAllAsync()
        //{

        //    return await _context.Etiquetas.
        //        OrderByDescending(t => t.FechaCreacion).
        //        ToArrayAsync();
        //}

        public async Task<PagedList<Etiqueta>> GetAllAsync(EtiquetasResourceParameters tagsParameters)
        {
            IQueryable<Etiqueta> query = _context.Etiquetas as IQueryable<Etiqueta>;

            //Busqueda en el nombre de la etiqueta
            if (!String.IsNullOrEmpty(tagsParameters.SearchQuery)) {

                var searchQuery = tagsParameters.SearchQuery.Trim();
                query = query.Where(t => t.Nombre.Contains(searchQuery));
            }

            query = query.OrderByDescending(t => t.FechaCreacion);

            return await PagedList<Etiqueta>.Create(query, tagsParameters.PageNumber, tagsParameters.PageSize);
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
