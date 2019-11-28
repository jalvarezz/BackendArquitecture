using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class GatewayException : ApplicationException
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

    [DataContract]
    public class GatewayFault
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

        public GatewayFault(string code, string message)
        {
            Message = message;
            Code = code;
        }
    }
}
