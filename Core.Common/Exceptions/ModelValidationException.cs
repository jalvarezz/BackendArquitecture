using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [DataContract]
    public class ModelValidationException: Exception
    {
        private string _Message;
        private List<string> _Errors;
        
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public List<string> Errors
        {
            get { return _Errors; }
            set { _Errors = value; }
        }

        public ModelValidationException(string message, List<string> error)
        {
            _Message = message;
            _Errors = error;
        }
    }
}
