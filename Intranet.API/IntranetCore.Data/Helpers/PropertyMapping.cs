using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntranetCore.Data.Helpers
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> Mappings { get; private set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappings)
        {
            Mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
            
        }
    }
}
