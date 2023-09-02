using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;
using MyBoilerPlate.Resources;
using Core.Common.Contracts;
using MyBoilerPlate.Business;
using System.Linq;
using MyBoilerPlate.Business.Settings;
using MyBoilerPlate.Web.Infrastructure;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class EngineService
    {
        public static void AddEngineServices(this IServiceCollection services, IConfiguration configuration)
        {
            var azureSettingValues = new AzureSetting();
            configuration.GetSection(nameof(AzureSetting)).Bind(azureSettingValues);

            if (!services.Any(x => x.ServiceType == typeof(AzureSetting)))
                services.AddSingleton(azureSettingValues);

            services.AddScoped<IBusinessEngineFactory, BusinessEngineFactory>();

            //Engines injection            
            var engineTypes =
                typeof(BusinessEngineFactory).Assembly
                                             .ExportedTypes
                                             .Where(x => typeof(IBusinessEngine).IsAssignableFrom(x) &&
                                                         !x.IsInterface &&
                                                         !x.IsAbstract).ToList();

            engineTypes.ForEach(engineType =>
            {
                services.AddScoped(engineType.GetInterface($"I{engineType.Name}"),
                                   engineType);
            });
        }
    }
}
