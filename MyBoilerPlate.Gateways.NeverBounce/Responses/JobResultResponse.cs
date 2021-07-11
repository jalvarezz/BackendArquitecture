using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class JobResultResponse : ResponseBase
    {
        public int TotalResults { get; set; }

        public int TotalPages { get; set; }

        public JobResultQueryResponse Query { get; set; }

        public List<ResultResponse> Results { get; set; }
    }
}
