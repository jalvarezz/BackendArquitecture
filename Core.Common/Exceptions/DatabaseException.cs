using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    public class DatabaseException : Exception
    {
        [DataMember]
        public string ResponseCode { get; set; }

        public DatabaseException(string message)
            : base(message)
        {
        }

        public DatabaseException(string code, string message)
            : base(message)
        {
            ResponseCode = code;
        }

        public DatabaseException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public DatabaseException(string code, string message, Exception exception)
            : base(message, exception)
        {
            ResponseCode = code;
        }
    }
}
