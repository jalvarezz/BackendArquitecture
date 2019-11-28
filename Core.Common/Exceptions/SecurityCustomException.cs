using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class SecurityCustomException : ApplicationException
    {
        [DataMember]
        public string ResponseCode { get; set; }

        public SecurityCustomException(string message)
            : base(message)
        {

        }

        public SecurityCustomException(string code, string message)
            : base(message)
        {
            ResponseCode = code;
        }

        public SecurityCustomException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }

    [DataContract]
    public class SecurityCustom
    {
        public string _Message;

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public SecurityCustom(string message)
        {
            Message = message;
        }
    }
}
