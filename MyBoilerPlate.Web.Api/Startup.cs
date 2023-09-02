#region usings
using System;
using System.Linq;
using MyBoilerPlate.Web.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Data.Initializers;
using Serilog;
using System.IO;
using MyBoilerPlate.Web.Infrastructure.Services;
using MyBoilerPlate.Web.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MyBoilerPlate.Web.Infrastructure.Extensions;
#endregion

namespace MyBoilerPlate.Web
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(WebApplicationBuilder builder, IConfiguration configuration, string environment)
        {
            var configurationFileName = environment != null ? $"appsettings.{environment}.json" : "appsettings.json";

            builder.Services.AddMvcServices(configuration);
            builder.Services.AddMapperService(configuration);
            builder.Services.AddSwaggerService(configuration);

            builder.AddLoggingService(configurationFileName);

            builder.Services.AddCacheService(configuration);
            builder.Services.AddCorsService(configuration);
            builder.Services.AddDataServices(configuration);
            builder.Services.AddDefaultServices(configuration);
            builder.Services.AddEngineServices(configuration);
            builder.Services.AddGatewayServices(configuration);
            builder.Services.AddHttpClientFactoryService(configuration);
            builder.Services.AddHttpContextService(configuration);
            builder.Services.AddRepositoryServices(configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void ConfigureApplication(IApplicationBuilder app, string environment)
        {
            if (environment == "Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                });
            }
            else
            {
                app.UseHsts();
            }

            // Prevent ClickJacking (Prevent rendering into an iFrame)
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });

            app.UseHsts();

            var corsSettings = app.ApplicationServices.GetService<CorsSetting>();

            app.UseCors((options) =>
            {
                options.WithOrigins(corsSettings.AllowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseAntiXssMiddleware();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckExtensions.WriteResponse
                });

                // SignalR Hubs
                //endpoints.MapHub<SomeHub>("/hubs/some");
                //endpoints.MapHub<AnotherHub>("/hubs/another");
            });
        }
    }
}
