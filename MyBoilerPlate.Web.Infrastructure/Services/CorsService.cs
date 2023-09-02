
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;

using System.Linq;
using MyBoilerPlate.Web.Infrastructure.Settings;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class CorsService
    {
        public static void AddCorsService(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSetting = new CorsSetting();
            configuration.GetSection(nameof(CorsSetting)).Bind(corsSetting);

            if (!services.Any(x => x.ServiceType == typeof(CorsSetting)))
                services.AddSingleton(corsSetting);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins(corsSetting.AllowedOrigins)
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}
