
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
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using MyBoilerPlate.Web.Infrastructure.Events;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MyBoilerPlate.Web.Infrastructure.Constants;

namespace MyBoilerPlate.Web.Api.Infrastructure.Services
{
    public static class MvcService
    {
        public static void AddMvcServices(this IServiceCollection services, IConfiguration configuration)
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

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                // To preserve the default behaviour, capture the original delegate to call later.
                var builtInFactory = options.InvalidModelStateResponseFactory;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var action = context.ActionDescriptor as ControllerActionDescriptor;

                    var serializableModelState = new SerializableError(context.ModelState);

                    var error = JsonSerializer.Serialize(serializableModelState);

                    var exception = new Exception($"action: {action.DisplayName}, error: {error}");

                    Log.Error(exception, "Model State Error");

                    return builtInFactory(context);
                };
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            });

            services.AddSingleton<JsonSerializerOptions>((s) => {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                return options;
            });

            services.AddOptions();

            services.AddDistributedMemoryCache();

            //Authentication
            var localJwtSettings = new LocalJwtSettings();

            configuration.GetSection(nameof(LocalJwtSettings)).Bind(localJwtSettings);

            services.AddSingleton<LocalJwtSettings>(localJwtSettings);

            services.AddAuthentication(SchemaNames.Bearer)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = authorizerSetting.Authority;
                        options.ApiName = authorizerSetting.ApiName;
                        options.ApiSecret = authorizerSetting.ApiSecret;
                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromMinutes(authorizerSetting.CacheDuration);
                        options.SupportedTokens = SupportedTokens.Jwt;
                        options.RequireHttpsMetadata = false;

                        options.JwtBearerEvents = new BoilerPlateJwtBearerEvents();
                    })
                    .AddJwtBearer("Local", options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = localJwtSettings.Issuer,
                            ValidAudience = localJwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(localJwtSettings.Key)),
                            NameClaimType = "name",
                        };

                        options.SecurityTokenValidators.Clear();
                        options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler
                        {
                            MapInboundClaims = false,
                            TokenLifetimeInMinutes = localJwtSettings.ExpiresOn
                        });

                        options.Events = new BoilerPlateJwtBearerEvents();
                    });

            services.AddMvcCore()
            .AddAuthorization(options =>
            {
                options.SetEndpointPolicies();
            })
            .AddApiExplorer();

            // Handler that validates Client Access
            services.AddScoped<IAuthorizationHandler, ClientApplicationAccessAuthorizationHandler>();
        }
    }
}
