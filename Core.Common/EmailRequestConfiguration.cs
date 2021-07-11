using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common
{
    public class EmailRequestConfiguration : IEmailRequestConfiguration
    {
        public string Endpoint { get; set; }
        public int Port { get; set; }
        public string Sender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
