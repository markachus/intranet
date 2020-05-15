using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IntranetCore.Api.Helper
{
    public class ArrayModelBinder : IModelBinder
    {
        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {
            //if the model is not an ienumerable
            if (!bindingContext.ModelMetadata.IsEnumerableType) {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var value = bindingContext.ValueProvider
                        .GetValue(bindingContext.ModelName).ToString();

            //If null or white space return succes (null)
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }


            //If value is not null or whitespace an is enumerable
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);


            //convert
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => converter.ConvertFromString(v.Trim()))
                .ToArray();

            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;

        }
    }
}
