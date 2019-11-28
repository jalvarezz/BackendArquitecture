using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    [Serializable]
    public class DataImportException : ApplicationException
    {
        public DataImportException(string message)
            : base(message)
        {
        }

        public DataImportException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
