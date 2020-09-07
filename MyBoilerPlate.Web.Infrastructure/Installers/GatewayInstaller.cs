using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;
using System.Linq;

namespace MyBoilerPlate.Web.Infrastructure.Installers
{
    public class GatewayInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Install here your settings and DI configuration to external web services (ex. Reporting services)

            ////var reportServerSettings = new ReportServerSetting();
            ////configuration.GetSection(nameof(ReportServerSetting)).Bind(reportServerSettings);

            ////if(!services.Any(x => x.ServiceType == typeof(ReportServerSetting)))
            ////    services.AddSingleton(reportServerSettings);

            ////// Gateways injection
            ////services.AddTransient<IReportServerGateway, ReportServerGateway>();
        }
    }
}
