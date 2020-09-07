
using Microsoft.AspNetCore.Http;
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
            if(value is IFormFile file && file.Length > _MaxFileSize)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { _MaxFileSize} bytes.";
        }
    }
}
