using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Core.Common.Contracts;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Web.Infrastructure;
using Core.Common.Services;
using MyBoilerPlate.Web.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System.IO;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class LoggingService
    {
        public static void AddLoggingService(this WebApplicationBuilder builder, string configurationFileName)
        {
            builder.Host.UseSerilog((ctx, lc) =>
            {
                lc.ReadFrom.Configuration(ctx.Configuration);
            }).ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(configurationFileName, true)
                    .AddEnvironmentVariables();

                config.Build();
            });
        }
    }
}
