
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
using MyBoilerPlate.Web.Infrastructure.Policy;
using Core.Common.Contracts;
using System.Security.Claims;
using System.Collections.Generic;

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
            var cultureInfo = new CultureInfo("es-DO");
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

                        options.JwtBearerEvents.OnTokenValidated = async (context) =>
                        {
                            var identity = context.Principal.Identity as ClaimsIdentity;

                            var currentUsernameClaim = identity.Claims.FirstOrDefault(x => x.Type == "name");

                            if (currentUsernameClaim == null)
                                return;

                            // load user specific data from database (read cache too)
                            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

                            var claims = await cacheService.GetCachedAsync("SF_" + currentUsernameClaim.Value, async () =>
                            {
                                //var profileService = context.HttpContext.RequestServices.GetRequiredService<IProfileService>();

                                var profileClaims = new List<Claim>();

                                // TODO: Set the user profile claims here

                                return profileClaims;
                            });

                            // add claims to the identity
                            identity.AddClaims(claims);
                        };
                    });

            services.AddMvcCore(options =>
            {
                var policy = ScopePolicy.Create("myboilerplate.fullaccess");
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "myboilerplateapi");
                });

                options.AddPolicy("ClientApplicationAccess", policy =>
                {
                    policy.Requirements.Add(new ClientApplicationAccessRequirement());
                });
            })
            .AddApiExplorer();

            // Handler that validates Client Access
            services.AddScoped<IAuthorizationHandler, ClientApplicationAccessAuthorizationHandler>();
        }
    }
}
