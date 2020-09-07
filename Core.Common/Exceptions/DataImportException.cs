using System;

namespace Core.Common.Exceptions
{
    public class DataImportException : Exception
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
