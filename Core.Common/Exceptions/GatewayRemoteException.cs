using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    public class GatewayRemoteException : Exception
    {
        [DataMember]
        public string ResponseCode { get; set; }

        public GatewayRemoteException(string message)
            : base(message)
        {
        }

        public GatewayRemoteException(string code, string message)
            : base(message)
        {
            ResponseCode = code;
        }

        public GatewayRemoteException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public GatewayRemoteException(string code, string message, Exception exception)
            : base(message, exception)
        {
            ResponseCode = code;
        }
    }
}
