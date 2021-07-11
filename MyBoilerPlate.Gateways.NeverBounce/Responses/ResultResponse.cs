using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ResultResponse
    {
        public Dictionary<string, object> Data { get; set; }

        public SingleResponse Verification { get; set; }
    }
}
