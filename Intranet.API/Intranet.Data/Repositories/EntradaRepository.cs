using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Intranet.Data.Repositories
{
    public class EntradaRepository : IEntradaRepository
    {
        private readonly IntranetDbContext _context;
        
        IntranetDbContext IEntradaRepository.Context { get { return _context;} }

        public EntradaRepository(IntranetDbContext context)
        {
            this._context = context;
        }

        void IEntradaRepository.Add(Entrada entrada)
        {
            _context.Entradas.Add(entrada);
        }

        void IEntradaRepository.AddEtiqueta(Entrada entrada, Etiqueta tag)
        {
            entrada.Etiquetas.Add(tag);
        }

        void IEntradaRepository.Delete(Entrada entrada)
        {
            _context.Entradas.Remove(entrada);
        }

        async Task<Entrada[]> IEntradaRepository.GetAllAsync(bool incluyeEtiquetas = false)
        {
            IQueryable<Entrada> query = _context.Entradas.Where(p => !p.Eliminado);
            if (incluyeEtiquetas)
            {
                query = query.Include(prop => prop.Etiquetas);
            }

            query = query.OrderByDescending(p => p.FechaCreacion);

            return await query.ToArrayAsync();
        }

        async Task<Entrada> IEntradaRepository.GetAsync(Guid EntradaId)
        {
            Entrada entrada = await _context.Entradas.
                                Where(p => p.Id == EntradaId).
                                Include(p => p.Etiquetas).
                                SingleOrDefaultAsync();
            return entrada;
        }

        void IEntradaRepository.RemoveEtiqueta(Entrada entrada, Etiqueta tag)
        {
            entrada.Etiquetas.Remove(tag);
        }

        async Task<bool> IEntradaRepository.SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
