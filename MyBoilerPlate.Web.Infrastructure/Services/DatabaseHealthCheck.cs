using Core.Common.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ConnectionStrings _ConnectionStrings;

        public DatabaseHealthCheck(ConnectionStrings connectionStrings)
        {
            _ConnectionStrings = connectionStrings;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using var connection = new SqlConnection(_ConnectionStrings?.MainConnection);

            try
            {
                await connection.OpenAsync(cancellationToken);
                return HealthCheckResult.Healthy("The database is in a healthy state.");
            }
            catch (SqlException)
            {
                return HealthCheckResult.Unhealthy("Unable to establish a connection to the database.");
            }
        }
    }
}
