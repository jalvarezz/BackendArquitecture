using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class JobCountsResponse
    {
        public int Completed { get; set; }

        public int UnderReview { get; set; }

        public int Queued { get; set; }

        public int Processing { get; set; }
    }
}
