using Core.Common;
using Core.Common.Contracts;
using Core.Common.DTOs;
using Core.Common.Exceptions;
using Core.Common.Extensions;
using MyBoilerPlate.Gateways.Firebase.Contracts;
using MyBoilerPlate.Gateways.Firebase.Responses;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Gateways.Firebase
{
    public class FirebaseGateway : IFirebaseGateway
    {
        private readonly FirebaseSettings _FirebaseSettings;

        public FirebaseGateway(FirebaseSettings firebaseSettings)
        {
            _FirebaseSettings = firebaseSettings;
        }

        public async ValueTask SendAsync(string deviceTokenId, string title, string body)
        {
            var data = new NotificationRequest
            {
                DeviceTokenId = deviceTokenId,
                Detail = new NotificationRequestDetail
                {
                    Title = title,
                    Body = body,
                    IconUrl = _FirebaseSettings.IconPath
                }
            };

            var jsonBody = data.ToJSON();

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, _FirebaseSettings.Host))
            {
                httpRequest.Headers.TryAddWithoutValidation("Authorization", _FirebaseSettings.ServerKey);
                httpRequest.Headers.TryAddWithoutValidation("Sender", _FirebaseSettings.SenderId);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.SendAsync(httpRequest);

                    if (!result.IsSuccessStatusCode)
                        Log.Error($"Error sending notification. Status Code: {result.StatusCode}");
                }
            }
        }
    }
}
