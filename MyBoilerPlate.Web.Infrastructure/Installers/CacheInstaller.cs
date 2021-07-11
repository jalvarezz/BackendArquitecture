using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyBoilerPlate.Web.Services;
using Core.Common.Contracts;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Web.Infrastructure;
using MyBoilerPlate.Web.Infrastructure.Services;
using Core.Common.Services;

namespace MyBoilerPlate.Web.Api.Infrastructure.Installers
{
    public class CacheInstaller : IInstaller
    {
        //NOTE: Redis is a Cache provider
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            var redisSection = configuration.GetSection(nameof(RedisCacheSettings));

            redisSection.Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            services.AddStackExchangeRedisCache((options) => {
                options.Configuration = redisCacheSettings.ConnectionString;
            });

            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
