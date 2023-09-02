
using Microsoft.AspNetCore.Http;
using MyBoilerPlate.Business.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MyBoilerPlate.Web.Infrastructure.Attributes
{
    public sealed class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _MaxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _MaxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && (file.Length / 1024) > _MaxFileSize)
            {
                var messageHandler = (IMessageHandler)validationContext.GetService(typeof(IMessageHandler));

                return new ValidationResult(string.Format(messageHandler.GetMessage("0189").Name, _MaxFileSize));
            }

            return ValidationResult.Success;
        }
    }
}
