using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure.Extensions
{
    public class HealthResult
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }
        public string Error { get; set; }
    }

    public class HealthCheckExtensions
    {
        public static Task WriteResponse(HttpContext context, HealthReport report)
        {
            var json = JsonConvert.SerializeObject(
                new
                {
                    Name = "MyBoilerPlate.Web.Api",
                    Status = report.Status.ToString(),
                    Duration = report.TotalDuration.ToString(),
                    Services = report.Entries
                        .Select(e =>
                            new HealthResult
                            {
                                Name = e.Key,
                                Duration = e.Value.Duration.ToString(),
                                Status = Enum.GetName(typeof(HealthStatus), e.Value.Status),
                                Error = e.Value.Exception?.Message
                            })
                        .ToList()
                }, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            context.Response.ContentType = MediaTypeNames.Application.Json;
            return context.Response.WriteAsync(json);
        }
    }
}
