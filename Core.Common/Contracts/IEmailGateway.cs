using Core.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IEmailGateway : IGateway
    {
        Task<bool> SendAsync(IEmailRequestConfiguration configuration,  
                             string subject, 
                             string body,
                             string recipients,
                             string cc = null, 
                             string bcc = null, 
                             IList<AttachmentDTO> attachments = null);
    }
}
