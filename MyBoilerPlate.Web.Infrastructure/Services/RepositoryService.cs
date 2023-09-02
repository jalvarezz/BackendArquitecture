using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Engines;
using Core.Common;
using MyBoilerPlate.Data.Contracts;
using MyBoilerPlate.Data;
using Core.Common.Contracts;
using System.Linq;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class RepositoryService
    {
        public static void AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IDataRepositoryFactory, DataRepositoryFactory>();

            //New instance for injection
            services.AddTransient(typeof(IDataRepository<>), typeof(Repository<>));

            //Custom Repositories injection            
            var repositoryTypes =
                typeof(Repository<>).Assembly
                                    .ExportedTypes
                                    .Where(x => typeof(IDataRepository).IsAssignableFrom(x) &&
                                                !x.IsInterface &&
                                                !x.IsAbstract).ToList();

            repositoryTypes.ForEach(repositoryType =>
            {
                var contract = repositoryType.GetInterface($"I{repositoryType.Name}");

                if (contract != null)
                    services.AddScoped(contract, repositoryType);
            });
        }
    }
}
