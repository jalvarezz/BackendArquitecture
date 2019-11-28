using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }

    [DataContract]
    public class NotFoundFault
    {
        public string _Message;

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public NotFoundFault(string message)
        {
            Message = message;
        }
    }
}
