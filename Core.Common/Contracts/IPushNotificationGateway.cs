using Core.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IPushNotificationGateway : IGateway
    {
        ValueTask SendAsync(string deviceTokenId, string title, string body);
    }
}
