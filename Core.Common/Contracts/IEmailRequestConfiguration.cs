using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Contracts
{
    public interface IEmailRequestConfiguration
    {
        string Endpoint { get; set; }
        int Port { get; set; }
        string Sender { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
