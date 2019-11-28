using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class EntityValidationException : ApplicationException
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

    [DataContract]
    public class EntityValidationFault
    {
        public string _Message;

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public EntityValidationFault(string message)
        {
            Message = message;
        }
    }
}
