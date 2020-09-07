using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    public class ProfileNotFoundException : Exception
    {
        [DataMember]
        public string ResponseCode { get; set; }

        public ProfileNotFoundException(string message)
            : base(message)
        {

        }

        public ProfileNotFoundException(string code, string message )
            : base(message)
        {
            ResponseCode = code;
        }

        public ProfileNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
