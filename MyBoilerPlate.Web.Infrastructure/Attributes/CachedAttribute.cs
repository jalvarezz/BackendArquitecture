using MyBoilerPlate.Web.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBoilerPlate.Web.Infrastructure.Services.Contracts;
using MyBoilerPlate.Business.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MyBoilerPlate.Web.Infrastructure.Attributes
{
    public sealed class CustomRegularExpressionAttribute : ValidationAttribute
    {
        private readonly string _IdMessage;

        private readonly RegularExpressionAttribute _InnerAttribute;

        public CustomRegularExpressionAttribute(string idMessage, string pattern)
        {
            _IdMessage = idMessage;

            _InnerAttribute = new RegularExpressionAttribute(pattern);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!_InnerAttribute.IsValid(value))
            {
                var messageHandler = (IMessageHandler)validationContext.GetService(typeof(IMessageHandler));

                throw new ArgumentException(messageHandler.GetMessage(_IdMessage).Name);
            }

            return ValidationResult.Success;
        }
    }
}