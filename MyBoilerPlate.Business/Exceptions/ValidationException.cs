using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;
using System;
using System.Runtime.Serialization;

namespace MyBoilerPlate.Business.Exceptions
{
    public class ValidationException : Exception
    {
        [DataMember]
        public string Code { get; set; }

        public ValidationException(KeyValueDTO<string> message)
            : base(message.Name)
        {
            Code = message.Id;
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string code, string message)
            : base(message)
        {
            Code = code;
        }

        public ValidationException(KeyValueDTO<string> message, Exception exception) : base(message.Name, exception)
        {
            Code = message.Id;
        }

        public ValidationException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public ValidationException(string code, string message, Exception exception)
            : base(message, exception)
        {
            Code = code;
        }
    }
}
