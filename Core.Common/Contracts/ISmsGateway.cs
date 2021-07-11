using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface ISmsGateway : IGateway
    {
        Task SendAsync(ISmsRequestConfiguration configuration, string messageTo, string messageBody);

        Task ValidatePhoneNumberAsync(ISmsRequestConfiguration configuration, string phoneNumber);
    }
}
