
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class CreditsInfoResponse
    {
        public int PaidCreditsUsed { get; set; }

        public int FreeCreditsUsed { get; set; }

        public int PaidCreditsRemaining { get; set; }

        public int FreeCreditsRemaining { get; set; }
    }
}
