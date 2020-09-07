
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
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using MyBoilerPlate.Web.Infrastructure.Settings;

namespace MyBoilerPlate.Web.Infrastructure.Installers
{
    //Sample implementation of the HttpClient Factory to communicate using OAUTH 2.0 via rest.
    public class HttpClientFactoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var authorizerSetting = new AuthorizerSetting();
            configuration.GetSection("Authorizer").Bind(authorizerSetting);

            if(!services.Any(x => x.ServiceType == typeof(AuthorizerSetting)))
                services.AddSingleton(authorizerSetting);

            var corsSetting = new CorsSetting();
            configuration.GetSection(nameof(CorsSetting)).Bind(corsSetting);

            if(!services.Any(x => x.ServiceType == typeof(CorsSetting)))
                services.AddSingleton(corsSetting);

            // http client services
            services.AddHttpClient("authorizer", c =>
            {
                ConfigureAuthorizerHttpClient(services, c, authorizerSetting, corsSetting);
            });
        }

        private void ConfigureAuthorizerHttpClient(IServiceCollection services,
                                                   HttpClient client,
                                                   AuthorizerSetting settings,
                                                   CorsSetting corsSettings)
        {
            // access the DI container
            var serviceProvider = services.BuildServiceProvider();

            // Find the HttpContextAccessor service
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

            // Get the bearer token from the request context (header)
            string bearerToken = null;

            bearerToken = httpContextAccessor.HttpContext.Request
                                  .Headers["Authorization"]
                                  .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));

            // Add authorization if found
            if(bearerToken != null)
                client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            // Add origin header
            if(corsSettings != null && corsSettings.AllowedOrigins.Length > 0)
            {
                client.DefaultRequestHeaders.Add("Origin", $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}");
            }

            //Other Settings
            client.BaseAddress = new Uri($"{settings.Authority}/");
            client.DefaultRequestHeaders.Add("Accept", "application/x-www-form-urlencoded");
        }
    }
}
