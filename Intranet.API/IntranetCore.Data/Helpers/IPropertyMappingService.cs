using System.Collections.Generic;

namespace IntranetCore.Data.Helpers
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsPropertyMappingValid<TSource, TDestination>(string sortFields);
    }
}