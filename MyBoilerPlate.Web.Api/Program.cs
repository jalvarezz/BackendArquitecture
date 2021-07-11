using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace MyBoilerPlate.Web
{
    public class Program
    {
        public static int Main(string[] args)
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

            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("Starting host...");
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}