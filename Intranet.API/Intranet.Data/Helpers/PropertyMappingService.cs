using Intranet.Data.Models;
using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.Data.Helpers
{
    public class PropertyMappingService : IPropertyMappingService
    {

        private Dictionary<string, PropertyMappingValue> entradaPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>(){"Id"})},
                {"Titulo", new PropertyMappingValue(new List<string>(){"Titulo"})},
                {"Contenido", new PropertyMappingValue(new List<string>(){"Contenido"})},
                {"FechaCreacion", new PropertyMappingValue(new List<string>(){"FechaCreacion"})}
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EntradaModel, Entrada>(entradaPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {

            var matchedMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchedMapping.Count() == 1)
            {
                return matchedMapping.First().Mappings;
            }

            throw new Exception($"Missing mapping for {typeof(TSource).Name} => {typeof(TDestination).Name}");
        }

        public bool IsPropertyMappingValid<TSource,TDestination>(string sortFields)
        {
            if (string.IsNullOrEmpty(sortFields)) return true;

            var mappingsDictionary = GetPropertyMapping<TSource, TDestination>();
            if (mappingsDictionary == null) return false;


            string[] orderBySplit = sortFields.Split(',');

            foreach (string orderByClause in orderBySplit)
            {

                var trimmedOrderByClaused = orderByClause.Trim();
                var idx = trimmedOrderByClaused.IndexOf(" ");
                var propertyName = idx == -1 ? 
                    trimmedOrderByClaused : trimmedOrderByClaused.Remove(idx);

                if (!mappingsDictionary.ContainsKey(propertyName)) return false;

            }
            return true;

        }
    }
}
