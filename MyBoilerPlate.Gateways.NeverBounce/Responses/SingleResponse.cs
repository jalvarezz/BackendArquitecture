using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SingleResponse : ResponseBase
    {
        public string Result { get; set; }

        public List<string> Flags { get; set; }

        public string SuggestedCorrection { get; set; }
    }
}
