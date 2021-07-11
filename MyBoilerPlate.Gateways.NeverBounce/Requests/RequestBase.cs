using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyBoilerPlate.Gateways.NeverBounce.Requests
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class RequestBase
    {
        public string Key { get; set; }
    }
}
