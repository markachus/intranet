using System.Collections.Generic;

namespace Intranet.Data.Helpers
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsPropertyMappingValid<TSource, TDestination>(string sortFields);
    }
}