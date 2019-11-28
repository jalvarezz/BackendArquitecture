using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class InvalidCaptchaException : ApplicationException
    {
        public InvalidCaptchaException(string message)
            : base(message)
        {
        }

        public InvalidCaptchaException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
