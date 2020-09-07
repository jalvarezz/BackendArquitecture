using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;
using MyBoilerPlate.Data.Contracts;
using MyBoilerPlate.Data;
using Core.Common.Contracts;

namespace MyBoilerPlate.Web.Infrastructure.Installers
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IDataRepositoryFactory, DataRepositoryFactory>();

            //New instance for injection
            services.AddTransient(typeof(IDataRepository<>), typeof(Repository<>));

            //Custom Repositories injection            
            services.AddTransient<IStoredProcedureRepository, StoredProcedureRepository>();
            services.AddTransient<IFunctionRepository, FunctionRepository>();
        }
    }
}
