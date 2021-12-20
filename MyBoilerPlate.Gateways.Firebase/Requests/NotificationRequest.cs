using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBoilerPlate.Gateways.Firebase.Responses
{
    public class NotificationRequest
    {
        #region Properties

        [JsonPropertyName("to")]
        public string DeviceTokenId { get; set; }

        [JsonPropertyName("notification")]
        public NotificationRequestDetail Detail { get; set; }

        #endregion
    }

    public class NotificationRequestDetail
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("icon")]
        public string IconUrl { get; set; }
    }
}
