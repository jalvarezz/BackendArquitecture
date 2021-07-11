using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Core.Common;
using Core.Common.Contracts;
using Core.Common.DTOs;
using Core.Common.Exceptions;
using MimeKit;
using MyBoilerPlate.Gateways.AWS.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Gateways.AWS
{
    public class AWSGateway : IAWSGateway
    {
        public async Task<bool> SendAsync(IEmailRequestConfiguration configuration,
                                          string subject,
                                          string body,
                                          string recipients,
                                          string cc = null,
                                          string bcc = null,
                                          IList<AttachmentDTO> attachments = null)
        {
            var message = new MimeMessage();

            // Set the sender and destinations
            message.From.Add(MailboxAddress.Parse(configuration.Sender));

            foreach (var destination in string.Join(',', recipients.Split(';')).Split(',').Select(x => x.Trim()))
            {
                message.To.Add(MailboxAddress.Parse(destination));
            }

            if (!string.IsNullOrEmpty(cc))
            {
                foreach (var ccAddress in string.Join(',', cc.Split(';')).Split(',').Select(x => x.Trim()))
                {
                    message.Cc.Add(MailboxAddress.Parse(ccAddress));
                }
            }

            if (!string.IsNullOrEmpty(bcc))
            {
                foreach (var bccAddress in string.Join(',', bcc.Split(';')).Split(',').Select(x => x.Trim()))
                {
                    message.Bcc.Add(MailboxAddress.Parse(bccAddress));
                }
            }

            // Set the subject
            message.Subject = subject;

            // Start the body building
            var bodyBuilder = new BodyBuilder()
            {
                HtmlBody = body,
                TextBody = body,
            };

            // Add the attachments
            if (attachments != null)
            {
                var ms = new MemoryStream();

                foreach (var attachment in attachments)
                {
                    bodyBuilder.Attachments.Add(attachment.FileName, new MemoryStream(attachment.Content));
                }
            }

            // Set the body
            message.Body = bodyBuilder.ToMessageBody();

            // Set the Raw message
            var streamMessage = new MemoryStream();
            message.WriteTo(streamMessage);

            var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(streamMessage) };

            AWSCredentials credentials = new BasicAWSCredentials(configuration.Username, configuration.Password);

            using (var ses = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.GetBySystemName(configuration.Endpoint)))
            {
                try
                {
                    await ses.SendRawEmailAsync(sendRequest);

                    Log.Information("Message sent!");

                    // Everything worked
                    return true;
                }
                catch (AccountSendingPausedException ex)
                {
                    throw new GatewayRemoteException("ASPE", ex.Message, ex);
                }
                catch (ConfigurationSetDoesNotExistException ex)
                {
                    throw new GatewayRemoteException("CSDNEE", ex.Message, ex);
                }
                catch (ConfigurationSetSendingPausedException ex)
                {
                    throw new GatewayRemoteException("CSSPE", ex.Message, ex);
                }
                catch (MailFromDomainNotVerifiedException ex)
                {
                    throw new GatewayRemoteException("MFDNVE", ex.Message, ex);
                }
                catch (MessageRejectedException ex)
                {
                    throw new GatewayRemoteException("MRE", ex.Message, ex);
                }
                catch (Exception ex) // Prevent the application from poping up an error
                {
                    throw new GatewayRemoteException("E", ex.Message, ex);
                }
            }
        }

        public async Task SendAsync(ISmsRequestConfiguration configuration, string messageTo, string messageBody)
        {
            // Send the SMS as an email
            var emailRequest = new EmailRequestConfiguration
            {
                Endpoint = configuration.Endpoint,
                Port = configuration.Port ?? 0,
                Username = configuration.AccountSid,
                Password = configuration.AuthorizationToken,
                Sender = configuration.Sender
            };

            await SendAsync(emailRequest, string.Empty, messageBody, messageTo);
        }

        public Task ValidatePhoneNumberAsync(ISmsRequestConfiguration configuration, string phoneNumber)
        {
            return Task.CompletedTask;
        }
    }
}
