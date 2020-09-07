using Newtonsoft.Json;

namespace MyBoilerPlate.Web.Models
{
    public class TokenResponseModel
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("expires_in")]
        public int expires_in { get; set; }

        [JsonProperty("Bearer")]
        public string Bearer { get; set; }

        [JsonProperty("scope")]
        public string scope { get; set; }
    }
}
