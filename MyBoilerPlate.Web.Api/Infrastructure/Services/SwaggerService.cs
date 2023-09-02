using MyBoilerPlate.Web.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MyBoilerPlate.Web.Api.Infrastructure.Services
{
    public static class SwaggerService
    {
        public static void AddSwaggerService(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiPe", Version = "v1" });
            });
        }
    }
}