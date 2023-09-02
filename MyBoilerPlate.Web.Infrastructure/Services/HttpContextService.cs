
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;

using System.Globalization;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Handlers;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class HttpContextService
    {
        public static void AddHttpContextService(this IServiceCollection services, IConfiguration configuration)
        {
            //Globally used objects
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
