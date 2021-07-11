using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace MyBoilerPlate.Gateways.NeverBounce.Requests
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class JobCreateRequest : RequestBase
    {
        public bool AutoStart { get; set; } = true;

        public bool AutoParse { get; set; } = true;

        public string InputLocation { get; } = "supplied";

        public List<object> Input { get; set; }
    }
}
