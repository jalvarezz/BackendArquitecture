using Microsoft.Extensions.Configuration;

namespace MyBoilerPlate.Tests
{
    public class TestConfigurationBuilder
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
