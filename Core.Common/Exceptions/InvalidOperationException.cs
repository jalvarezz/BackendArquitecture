using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [DataContract]
    public class InvalidOperationFault
    {
        public string _Message;

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public InvalidOperationFault(string message)
        {
            Message = message;
        }
    }
}
