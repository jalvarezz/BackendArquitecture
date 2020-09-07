using System;

namespace Core.Common.Exceptions
{
    public class NotFoundException : Exception
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
}
