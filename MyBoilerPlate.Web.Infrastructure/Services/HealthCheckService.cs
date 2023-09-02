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
    public static class HealthCheckService
    {
        public static void AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database_check");
        }
    }
}
