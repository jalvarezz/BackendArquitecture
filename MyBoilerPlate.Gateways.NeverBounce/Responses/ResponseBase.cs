using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ResponseBase
    {
        public string Status { get; set; }

        public int ExecutionTime { get; set; }
    }
}
