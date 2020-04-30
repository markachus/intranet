using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.Data.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, 
            string OrderBy, 
            Dictionary<string, PropertyMappingValue> mappingDictionary) {

            if (source == null) throw new ArgumentNullException(nameof(source));

            if (mappingDictionary == null) throw new ArgumentNullException(nameof(mappingDictionary));

            if (string.IsNullOrEmpty(OrderBy)) return source;

            string[] orderBySplit = OrderBy.Split(',');
            string orderByString = String.Empty;

            foreach (string orderByClause in orderBySplit.Reverse()) {

                var trimmedOrderByClaused = orderByClause.Trim();

                var descending = trimmedOrderByClaused.EndsWith(" desc");
                var idx = trimmedOrderByClaused.IndexOf(" ");
                var propertyName = idx == -1? trimmedOrderByClaused : trimmedOrderByClaused.Remove(idx);


                if (!mappingDictionary.ContainsKey(propertyName)) throw new Exception($"Missing property mapping for {propertyName}");
                var mappingValue = mappingDictionary[propertyName];

                if (mappingValue == null) throw new ArgumentNullException("mappingValue");
                
                //Revert order
                if (mappingValue.Revert) descending = !descending;

                foreach (string mappedProperty in mappingValue.DestinationProperties) { 
                
                    orderByString = orderByString + 
                        (string.IsNullOrEmpty(orderByString) ? string.Empty : ", ")
                        + mappedProperty
                        + (descending ? " descending" : " ascending");
                }

            }

            return source.OrderBy(orderByString);
        
        }
    }
}
