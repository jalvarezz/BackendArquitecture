using Microsoft.Extensions.Configuration;
using Serilog;

namespace MyBoilerPlate.Tests
{
    public class TestConfigurationBuilder
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(outputPath)
                                .AddJsonFile("appsettings.Development.json", optional: true)
                                .AddEnvironmentVariables()
                                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return configuration;
        }
    }
}
