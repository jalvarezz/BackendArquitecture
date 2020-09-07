using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;
using MyBoilerPlate.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBoilerPlate.Web.Infrastructure.Settings;

namespace MyBoilerPlate.Web.Infrastructure.Installers
{
    public class DataInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionStrings = new ConnectionStrings();
            configuration.GetSection(nameof(ConnectionStrings)).Bind(connectionStrings);

            if(!services.Any(x => x.ServiceType == typeof(ConnectionStrings)))
                services.AddSingleton(connectionStrings);

            var mainConnectionString = connectionStrings.MainConnection;

            //One instance per request
            services.AddDbContext<SampleDataContext>(options =>
            {
                //NOTE: There is an AppSettings.json for TEST Projects picks the one of their project

                var connection = new SqlConnection(mainConnectionString);

                options.UseSqlServer(connection);
            });
        }
    }
}
