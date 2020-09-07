using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    public class SecurityCustomException : Exception
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
}
