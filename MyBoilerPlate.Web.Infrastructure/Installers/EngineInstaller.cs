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

namespace MyBoilerPlate.Web.Api.Infrastructure.Installers
{
    public class EngineInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var azureSettingValues = new AzureSetting();
            configuration.GetSection(nameof(AzureSetting)).Bind(azureSettingValues);

            if(!services.Any(x => x.ServiceType == typeof(AzureSetting)))
                services.AddSingleton(azureSettingValues);

            services.AddScoped<IBusinessEngineFactory, BusinessEngineFactory>();

            //Engines injection            
            services.AddTransient<IEmployeeEngine, EmployeeEngine>();
        }
    }
}
