using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class ProfileNotFoundException : ApplicationException
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

    [DataContract]
    public class ProfileNotFoundFault
    {
        public string _Message;

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public ProfileNotFoundFault(string message)
        {
            Message = message;
        }
    }
}
