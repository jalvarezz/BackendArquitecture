
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;



using System.Globalization;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Web.Infrastructure;

namespace MyBoilerPlate.Web.Api.Infrastructure.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var authorizerSetting = new AuthorizerSetting();
            configuration.GetSection("Authorizer").Bind(authorizerSetting);

            if(!services.Any(x => x.ServiceType == typeof(AuthorizerSetting)))
                services.AddSingleton(authorizerSetting);

            //Antiforgery Setup
            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            //Add Culture
            var cultureInfo = new CultureInfo("es-PR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.AddControllers();

            services.AddOptions();

            services.AddDistributedMemoryCache();

            //Authentication
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = authorizerSetting.Authority;
                        options.ApiName = authorizerSetting.ApiName;
                        options.ApiSecret = authorizerSetting.ApiSecret;
                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromMinutes(authorizerSetting.CacheDuration);
                        options.SupportedTokens = SupportedTokens.Jwt;
                        options.RequireHttpsMetadata = false;
                    });

            services.AddMvcCore(options =>
            {
                var policy = ScopePolicy.Create("mipeapi.fullaccess");
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddAuthorization()
            .AddApiExplorer();
        }
    }
}
