using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Intranet.API.Data;
using Intranet.Data.Helpers;

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

        async Task<PagedList<Entrada>> IEntradaRepository.GetAllAsync(EntradasResourceParameters param, bool incluyeEtiquetas = false)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));

            IQueryable<Entrada> query = _context.Entradas.Where(p => !p.Eliminado);
            if (incluyeEtiquetas)
            {
                query = query.Include(prop => prop.Etiquetas);
            }

            query = query.OrderByDescending(p => p.FechaCreacion);

            return await PagedList<Entrada>.Create(query, param.PageNumber, param.PageSize);
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
