using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace MyBoilerPlate.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            SetupLogger();

            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .UseSerilog()
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                       }).ConfigureAppConfiguration(context =>
                       {
                           context.AddEnvironmentVariables();
                       });
        }

        private static void SetupLogger()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var confFileName = environment != null ? $"appsettings.{environment}.json" : "appsettings.json";

            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile(confFileName)
                                    .AddEnvironmentVariables()
                                    .Build();

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
        }
    }
}
