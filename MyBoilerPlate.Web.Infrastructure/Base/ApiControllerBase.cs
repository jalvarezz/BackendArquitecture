using Core.Common.Contracts;
using MyBoilerPlate.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MyBoilerPlate.Web.Infrastructure
{
    public abstract class ApiControllerBase : Controller
    {
        protected static Dictionary<string, IEnumerable<string>> GetModelErrors(ModelStateDictionary modelState)
        {
            var errors = modelState.ToDictionary(k => k.Key, v => v.Value.Errors.Select(s => s.ErrorMessage));

            return errors;
        }
    }
}
