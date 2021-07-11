using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyBoilerPlate.Gateways.NeverBounce.Responses
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class JobResultQueryResponse
    {
        public int JobId { get; set; }

        public int Valids { get; set; }

        public int Invalids { get; set; }

        public int Disposables { get; set; }

        public int Catchalls { get; set; }

        public int Unknowns { get; set; }

        public int Page { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
