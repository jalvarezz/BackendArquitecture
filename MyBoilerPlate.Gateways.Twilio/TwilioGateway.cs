using Core.Common.Contracts;
using MyBoilerPlate.Gateways.Twilio.Contracts;
using Serilog;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Lookups.V1;
using Twilio.Exceptions;
using Core.Common.Exceptions;

namespace MyBoilerPlate.Gateways.Twilio
{
    public class TwilioGateway : ITwilioGateway
    {
         /// <summary>
        /// Send an SMS Async using Twilio API.
        /// </summary>
        /// <param name="configuration"> The Configuration which will be provided to the twilio api. Ex.(Sid, AuthorizationToken)</param>
        /// <param name="messageTo">The destination phone number who will receive the sms</param>
        /// <param name="messageBody"> The Content of the message body</param>
        /// <returns>True if the send operation succeed false otherwise</returns>
        public async Task SendAsync(ISmsRequestConfiguration configuration, string messageTo, string messageBody)
        {
            if (string.IsNullOrWhiteSpace(messageBody))
                throw new GatewayException("Content of the message is empty");
            if (messageBody.Length >= 1600)
                throw new GatewayException("Message cannot be longer than 1600 chars");

            try
            {
                TwilioClient.Init(configuration.AccountSid, configuration.AuthorizationToken);

                await MessageResource.CreateAsync(
                    from: new PhoneNumber(configuration.Sender), 
                    to: new PhoneNumber(messageTo), 
                    body: messageBody);

                Log.Information("Message sent!");
            }
            catch (ApiException ex)
            {
                throw new GatewayRemoteException("AE", ex.Message, ex);
            }
            catch (CertificateValidationException ex)
            {
                throw new GatewayRemoteException("CVE", ex.Message, ex);
            }
            catch (RestException ex)
            {
                throw new GatewayRemoteException("RE", ex.Message, ex);
            }
            catch (TwilioException ex)
            {
                throw new GatewayRemoteException("TE", ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new GatewayRemoteException("E", ex.Message, ex);
            }
        }

        /// <summary>
        /// Validate a Phone Number Async using Twilio API.
        /// </summary>
        /// <param name="configuration"> The Configuration which will be provided to the twilio api. Ex.(Sid, AuthorizationToken)</param>
        /// <param name="phoneNumber">The phone number who will be Validated</param>
        /// <param name="messageBody"> The Content of the message body</param>
        /// <returns>True if the send operation succeed false otherwise</returns>
        public async Task ValidatePhoneNumberAsync(ISmsRequestConfiguration configuration, string phoneNumber)
        {
            try
            {
                TwilioClient.Init(configuration.AccountSid, configuration.AuthorizationToken);
                var result = await PhoneNumberResource.FetchAsync(
                    pathPhoneNumber: new PhoneNumber(phoneNumber)
                );
            }
            catch (ApiException ex)
            {
                Log.Error(ex, ex.Message, ex);
            }
            catch (CertificateValidationException ex)
            {
                Log.Error(ex, ex.Message, ex);
            }
            catch (RestException ex)
            {
                Log.Error(ex, ex.Message, ex);
            }
            catch (TwilioException ex)
            {
                Log.Error(ex, ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex);
            }
        }
    }
}
