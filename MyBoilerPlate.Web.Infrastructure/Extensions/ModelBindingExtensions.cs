using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace MyBoilerPlate.Web.Infrastructure.Extensions
{
    public static class ModelBindingExtensions
    {
        public static List<string> GetErrorsList(this ModelStateDictionary modelState)
        {
            List<string> result = new List<string>();

            foreach(var modelValues in modelState.Values)
                foreach(var error in modelValues.Errors)
                    result.Add(error.ErrorMessage);

            return result;
        }

        public static Dictionary<string, string[]> GetErrorsDictionary(this ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }
    }
}
