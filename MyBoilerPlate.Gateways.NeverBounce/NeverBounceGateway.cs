using Core.Common.Contracts;
using Core.Common.Exceptions;
using Core.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MyBoilerPlate.Gateways.NeverBounce.Contracts;
using MyBoilerPlate.Gateways.NeverBounce.Enums;
using MyBoilerPlate.Gateways.NeverBounce.Requests;
using MyBoilerPlate.Gateways.NeverBounce.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyBoilerPlate.Gateways.NeverBounce
{
    public class NeverBounceGateway : INeverBounceGateway
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly NeverBounceSetting _NeverBounceSetting;

        public NeverBounceGateway(IHttpClientFactory httpClientFactory, NeverBounceSetting neverBounceSetting)
        {
            _HttpClientFactory = httpClientFactory;
            _NeverBounceSetting = neverBounceSetting;
        }

        private async Task<string> ParseResponse(HttpResponseMessage response, bool isJobResponse = false)
        {
            if (response.StatusCode > HttpStatusCode.InternalServerError)
            {
                throw new GatewayRemoteException(string.Format(
                    "We were unable to complete your request. "
                    + "The following information was supplied: {0}"
                    + "\n\n(Internal error[status {1}])", response.StatusCode, response.StatusCode.GetHashCode()
                ));
            }

            if (response.StatusCode > HttpStatusCode.BadRequest)
            {
                throw new GatewayRemoteException(string.Format(
                    "We were unable to complete your request. "
                    + "The following information was supplied: {0}"
                    + "\n\n(Request error[status {1}])", response.StatusCode, response.StatusCode.GetHashCode()
                ));
            }

            var contentType = response.Content.Headers.ContentType.ToString();
            var data = await response.Content.ReadAsStringAsync();

            if (contentType == Application.Json)
            {
                JObject token;
                try
                {
                    token = JObject.Parse(data);
                }
                catch (Exception)
                {
                    throw new GatewayRemoteException(string.Format(
                        "The response from NeverBounce was unable "
                        + "to be parsed as json. Try the request "
                        + "again, if this error persists "
                        + "let us know at support@neverbounce.com. "
                        + "The following information was supplied: {0} "
                        + "\n\n(Internal error)", data));
                }

                Enum.TryParse((string)token.SelectToken("status"), out StatusCode status);
                if (status != StatusCode.success)
                {
                    switch (status)
                    {
                        case StatusCode.auth_failure:
                            throw new GatewayRemoteException(string.Format(
                                "We were unable to authenticate your request. "
                                + "The following information was supplied: {0} "
                                + "\n\n(auth_failure)", token.SelectToken("message")));
                        case StatusCode.temp_unavail:
                            throw new GatewayRemoteException(string.Format(
                                "We were unable to complete your request. "
                                + "The following information was supplied: {0} "
                                + "\n\n(temp_unavail)", token.SelectToken("message")));

                        case StatusCode.throttle_triggered:
                            throw new GatewayRemoteException(string.Format(
                                "We were unable to complete your request. "
                                + "The following information was supplied: {0} "
                                + "\n\n(throttle_triggered)", token.SelectToken("message")));

                        case StatusCode.bad_referrer:
                            throw new GatewayRemoteException(string.Format(
                                "We were unable to complete your request. "
                                + "The following information was supplied: {0}"
                                + "\n\n(bad_referrer)", token.SelectToken("message")));
                        default:
                            var message = token.SelectToken("message").ToString();

                            if (!isJobResponse || (isJobResponse && !message.Equals("Results are not currently available for this job.")))
                            {
                                throw new GatewayRemoteException(string.Format(
                                    "We were unable to complete your request. "
                                    + "The following information was supplied: {0}"
                                    + "\n\n({1})", message, token.SelectToken("status")));
                            }
                            break;
                    }
                }
            }
            else
            {
                throw new GatewayRemoteException(string.Format(
                    "The response from NeverBounce was has a data type of \"{0}\", but \"{1}\" was expected."
                    + "The following information was supplied: {2}"
                    + "\n\n(Internal error[status {3}])", contentType, Application.Json, response.StatusCode, response.StatusCode.GetHashCode()
                ));
            }

            return data;
        }

        public async Task<bool> AccountHaveCreditAsync()
        {
            var client = _HttpClientFactory.CreateClient("bounce");

            var result = await client.GetAsync($"account/info?key={_NeverBounceSetting.Key}");
            var response = JsonConvert.DeserializeObject<AccountInfoResponse>(await ParseResponse(result));

            if (response.Status == StatusCode.success.GetDisplayName())
            {
                if (response.SubscriptionType == SubscriptionType.monthly_usage.GetDisplayName())
                {
                    return true;
                }
                else if (response.SubscriptionType == SubscriptionType.pay_as_you_go.GetDisplayName() &&
                    (response.CreditsInfo.PaidCreditsRemaining > 0 || response.CreditsInfo.FreeCreditsRemaining > 0))
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<string> CheckAsync(string email)
        {
            if (!(await AccountHaveCreditAsync())) return string.Empty;

            var client = _HttpClientFactory.CreateClient("bounce");

            var result = await client.GetAsync($"single/check?key={_NeverBounceSetting.Key}&email={email}");
            var response = JsonConvert.DeserializeObject<SingleResponse>(await ParseResponse(result));

            return response.Result;
        }

        public async Task<List<string>> CheckAsync(List<string> emails)
        {
            if (!(await AccountHaveCreditAsync())) return null;

            var response = new List<string>();
            var input = new List<object>();

            emails.ForEach(email => input.Add(new { email }));

            var client = _HttpClientFactory.CreateClient("bounce");

            var content = new StringContent(JsonConvert.SerializeObject(new JobCreateRequest()
            {
                Key = _NeverBounceSetting.Key,
                Input = input
            }), Encoding.UTF8, Application.Json);

            var jobCreationResult = await client.PostAsync($"jobs/create", content);
            var jobCreationResponse = JsonConvert.DeserializeObject<JobCreateResponse>(await ParseResponse(jobCreationResult));

            HttpResponseMessage jobResult = null;
            JobResultResponse jobResponse = null;

            while (jobResponse == null || jobResponse.Status == StatusCode.general_failure.GetDisplayName())
            {
                jobResult = await client.GetAsync($"jobs/results?key={_NeverBounceSetting.Key}&job_id={jobCreationResponse.JobId}&items_per_page=1000");
                jobResponse = JsonConvert.DeserializeObject<JobResultResponse>(await ParseResponse(jobResult, true));
                Thread.Sleep(TimeSpan.FromSeconds(1.5));
            }

            jobResponse.Results.ForEach(result => response.Add(result.Verification.Result));

            return response;
        }
    }
}
