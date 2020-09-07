using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    public class EntityValidationException : Exception
    {
        [DataMember]
        public string ResponseCode { get; set; }

        public EntityValidationException(string message)
            : base(message)
        {

        }

        public EntityValidationException(string code, string message )
            : base(message)
        {
            ResponseCode = code;
        }

        public EntityValidationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
