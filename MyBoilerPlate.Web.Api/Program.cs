using Microsoft.AspNetCore.Builder;
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
                Log.Information($"Using settings file {confFileName}");
                Log.Information("Configuring host...");

                var builder = WebApplication.CreateBuilder(args);

                Startup.ConfigureServices(builder, configuration, environment);

                var app = builder.Build();

                Startup.ConfigureApplication(app, environment);

                Log.Information("Starting host...");

                app.Run();

                Log.Information("Host stopped successfully...");

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
    }
}