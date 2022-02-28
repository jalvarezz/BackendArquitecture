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
using MyBoilerPlate.Web.Infrastructure.Installers;
using MyBoilerPlate.Data.Initializers;
using Serilog;
using System.IO;
#endregion

namespace MyBoilerPlate.Web
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(WebApplicationBuilder builder, IConfiguration configuration, string environment)
        {
            var confFileName = environment != null ? $"appsettings.{environment}.json" : "appsettings.json";

            builder.Host.UseSerilog((ctx, lc) =>
            {
                lc.ReadFrom.Configuration(ctx.Configuration);
            }).ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(confFileName, true)
                    .AddEnvironmentVariables();

                config.Build();
            });

            var commonInstallers = typeof(DefaultInstaller).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            commonInstallers.ForEach(installer => installer.InstallServices(builder.Services, configuration));

            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x => 
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                            .Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(builder.Services, configuration));
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
            });
        }
    }
}
