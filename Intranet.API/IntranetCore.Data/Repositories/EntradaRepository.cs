using IntranetCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using IntranetCore.API.Data;
using IntranetCore.Data.Helpers;
using IntranetCore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IntranetCore.Data.Repositories
{
    public class EntradaRepository : IEntradaRepository
    {
        private readonly IntranetDbContext _context;
        private readonly IPropertyMappingService _mappingService;

        IntranetDbContext IEntradaRepository.Context { get { return _context;} }

        public EntradaRepository(IntranetDbContext context, IPropertyMappingService mappingService)
        {
            this._context = context;
            this._mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }

        void IEntradaRepository.Add(Entrada entrada)
        {
            _context.Entradas.Add(entrada);
        }

        void IEntradaRepository.AddEtiqueta(Entrada entrada, Etiqueta tag)
        {
            var assoc = new EntradaEtiqueta { Entrada = entrada, Etiqueta = tag };
            entrada.Etiquetas.Add(assoc);
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
                query = query
                    .Include(prop => prop.Etiquetas) //incluye List<EntradaEtiqueta>
                    .ThenInclude( t => t.Etiqueta); // incluye EntradaEtiqueta.Etiqueta
            }

            if (!string.IsNullOrEmpty(param.SearchQuery)) {
                var searchQuery = param.SearchQuery.Trim();

                query = query.Where(e => e.Titulo.Contains(searchQuery) ||
                    e.Contenido.Contains(searchQuery));
            }

            var entradaPropertyMappingDictionary = _mappingService.GetPropertyMapping<EntradaModel, Entrada>();
            query = query.ApplySort(param.OrderBy, entradaPropertyMappingDictionary);

            return await PagedList<Entrada>.Create(query, param.PageNumber, param.PageSize);
        }

        async Task<Entrada> IEntradaRepository.GetAsync(Guid EntradaId)
        {
            Entrada entrada = await _context.Entradas.
                                Where(p => p.Id == EntradaId).
                                Include(p => p.Etiquetas).
                                ThenInclude(p => p.Etiqueta)
                                .SingleOrDefaultAsync();
            return entrada;
        }

        void IEntradaRepository.RemoveEtiqueta(Entrada entrada, Etiqueta tag)
        {
            var tagToremove = entrada.Etiquetas
                .Where(t => t.Etiqueta.Nombre == tag.Nombre)
                .SingleOrDefault();

            entrada.Etiquetas.Remove(tagToremove);
        }

        async Task<bool> IEntradaRepository.SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
