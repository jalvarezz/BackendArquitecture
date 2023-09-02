using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Core.Common.Contracts;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Web.Infrastructure;
using Core.Common.Services;
using MyBoilerPlate.Web.Infrastructure.Services.Contracts;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class CacheService
    {
        public static void AddCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            var redisSection = configuration.GetSection(nameof(RedisCacheSettings));

            redisSection.Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            services.AddStackExchangeRedisCache((options) =>
            {
                options.Configuration = redisCacheSettings.ConnectionString;
            });

            services.AddSingleton<ICacheService, Core.Common.Services.CacheService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
