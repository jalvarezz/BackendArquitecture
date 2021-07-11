using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common
{
    public class SMSRequestConfiguration : ISmsRequestConfiguration
    {
        public string AccountSid { get; set; }
        public string AuthorizationToken { get; set; }
        public string Endpoint { get; set; }
        public int? Port { get; set; }
        public string Sender { get; set; }
    }
}
