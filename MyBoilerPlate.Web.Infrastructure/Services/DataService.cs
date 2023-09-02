using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;
using MyBoilerPlate.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Data.Oracle;
using Core.Common.Settings;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public static class DataService
    {
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStrings = new ConnectionStrings();
            configuration.GetSection(nameof(ConnectionStrings)).Bind(connectionStrings);

            if (!services.Any(x => x.ServiceType == typeof(ConnectionStrings)))
                services.AddSingleton(connectionStrings);

            var mainConnectionString = connectionStrings.MainConnection;
            var oracleConnectionString = connectionStrings.OracleConnection;

            //One instance per request
            services.AddDbContext<SampleDataContext>(options =>
            {
                var connection = new SqlConnection(mainConnectionString);

                options.UseSqlServer(connection);
            });

            //One instance per request
            services.AddDbContext<SampleOracleDataContext>(options =>
            {
                options.UseOracle(oracleConnectionString, options => options
                    .UseOracleSQLCompatibility("11"));
            });
        }
    }
}
