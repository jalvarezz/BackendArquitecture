using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AccountInfoResponse : ResponseBase
    {
        public string SubscriptionType { get; set; }

        public CreditsInfoResponse CreditsInfo { get; set; }

        public JobCountsResponse JobCounts { get; set; }
    }
}
