using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Data;
using Core.Common.Contracts;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Web.Infrastructure;

namespace MyBoilerPlate.Tests.Installers
{
    public class FakeRepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IDataRepositoryFactory, DataRepositoryFactory>();
            
            //New instance for injection
            services.AddScoped(typeof(IDataRepository<Employee>), typeof(EmployeeFakeRepository));
            services.AddScoped(typeof(IDataRepository<EmployeeType>), typeof(EmployeeTypeFakeRepository));
        }
    }
}
