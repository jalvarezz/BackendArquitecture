using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class AuthorizationValidationException : ApplicationException
    {
        public AuthorizationValidationException(string message)
            : base(message)
        {
        }

        public AuthorizationValidationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
