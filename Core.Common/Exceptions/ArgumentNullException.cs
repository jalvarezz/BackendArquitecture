using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [DataContract]
    public class ArgumentNullFault:Exception
    {
        public string _Message;
        public string _ParamName;

        [DataMember]
        public override string Message
        {
            get { return _Message; }
            //set { _Message = value; }
        }

        public string ParamName
        {
            get { return _ParamName; }
            set { _ParamName = value; }
        }

        public ArgumentNullFault(string message, string paramname)
        {
            _Message = message;
            ParamName = paramname;
        }
    }
}
