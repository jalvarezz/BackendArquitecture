using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface ISmsRequestConfiguration
    {
        string AccountSid { get; set; }
        string AuthorizationToken { get; set; }
        string Endpoint { get; set; }
        int? Port { get; set; }
        string Sender { get; set; }
    }
}
