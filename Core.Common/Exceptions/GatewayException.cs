using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    public class GatewayException : Exception
    {
        [DataMember]
        public string ResponseCode { get; set; }

        public GatewayException(string message)
            : base(message)
        {
        }

        public GatewayException(string code, string message)
            : base(message)
        {
            ResponseCode = code;
        }

        public GatewayException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public GatewayException(string code, string message, Exception exception)
            : base(message, exception)
        {
            ResponseCode = code;
        }
    }
}
