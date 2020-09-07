using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using MyBoilerPlate.Web.Api.Infrastructure.AutoMapper;
using MyBoilerPlate.Web.Infrastructure;
using MyBoilerPlate.Web.Infrastructure.AutoMapper;

namespace MyBoilerPlate.Web.Api.Infrastructure.Installers
{
    public class AutomapperInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
                cfg.AddProfile(new AutoMapperCommonProfileConfiguration());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
