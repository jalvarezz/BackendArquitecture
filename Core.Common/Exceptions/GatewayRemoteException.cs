using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class GatewayRemoteException : ApplicationException
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

    [DataContract]
    public class GatewayRemoteFault
    {
        public string _Code;
        public string _Message;

        [DataMember]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public GatewayRemoteFault(string code, string message)
        {
            Message = message;
            Code = code;
        }
    }
}
