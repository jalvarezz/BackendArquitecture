using Core.Common.Contracts;
using Core.Common.DTOs;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBoilerPlate.Business;
using MyBoilerPlate.Web.Infrastructure;
using System;
using System.IO;
using System.Reflection;

namespace MyBoilerPlate.Web.Api.Infrastructure.Services
{
    public static class MapperService
    {
        public static void AddMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Custom configuration goes here
            config.Scan(Assembly.GetAssembly(typeof(Startup)),
                        Assembly.GetAssembly(typeof(IBusinessEngineFactory)),
                        Assembly.GetAssembly(typeof(BusinessEngineFactory)));

            TypeAdapterConfig<IFormFile, AttachmentDTO>.NewConfig()
                .Map(dest => dest.FileName, source => source != null ? source.FileName : null)
                .Map(dest => dest.Content, source => source != null ? FileToBytes(source) : null);

            // Pending => "IgnoreNonMapped" - Manual configuration

            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();
        }

        private static byte[] FileToBytes(IFormFile file)
        {
            if (file == null)
                return new byte[0];

            byte[] documentfileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                documentfileBytes = ms.ToArray();
            }

            return documentfileBytes;
        }
    }

    public class ServiceMapper : Mapper
    {
        internal const string _DIKEY = "Mapster.DependencyInjection.sp";
        private readonly IServiceProvider _ServiceProvider;

        public ServiceMapper(IServiceProvider serviceProvider, TypeAdapterConfig config) : base(config)
        {
            _ServiceProvider = serviceProvider;
        }

        public override TypeAdapterBuilder<TSource> From<TSource>(TSource source)
        {
            return base.From(source)
                .AddParameters(_DIKEY, _ServiceProvider);
        }

        public override TDestination Map<TDestination>(object source)
        {
            using var scope = new MapContextScope();
            scope.Context.Parameters[_DIKEY] = _ServiceProvider;
            return base.Map<TDestination>(source);
        }

        public override TDestination Map<TSource, TDestination>(TSource source)
        {
            using var scope = new MapContextScope();
            scope.Context.Parameters[_DIKEY] = _ServiceProvider;
            return base.Map<TSource, TDestination>(source);
        }

        public override TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            using var scope = new MapContextScope();
            scope.Context.Parameters[_DIKEY] = _ServiceProvider;
            return base.Map(source, destination);
        }

        public override object Map(object source, Type sourceType, Type destinationType)
        {
            using var scope = new MapContextScope();
            scope.Context.Parameters[_DIKEY] = _ServiceProvider;
            return base.Map(source, sourceType, destinationType);
        }

        public override object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            using var scope = new MapContextScope();
            scope.Context.Parameters[_DIKEY] = _ServiceProvider;
            return base.Map(source, destination, sourceType, destinationType);
        }
    }
}
