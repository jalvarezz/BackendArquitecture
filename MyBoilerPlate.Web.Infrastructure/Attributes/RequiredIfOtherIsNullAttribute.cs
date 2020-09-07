using MyBoilerPlate.Business.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBoilerPlate.Web.Infrastructure.Attributes
{
    public sealed class RequiredIfOtherIsNullAttribute : ValidationAttribute, IClientModelValidator
    {
#pragma warning disable IDE0052 // Remove unread private members
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }
#pragma warning restore IDE0052 // Remove unread private members
        private string IdMessage { get; set; }

        public RequiredIfOtherIsNullAttribute(string propertyName, object desiredvalue, string idmessage)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.IdMessage = idmessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var messageHandler = (IMessageHandler)validationContext.GetService(typeof(IMessageHandler));

            if(value != null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(messageHandler.GetMessage(IdMessage).Name);
            }
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var messageHandler = (IMessageHandler)context.ActionContext
                                                         .HttpContext
                                                         .RequestServices
                                                         .GetService(typeof(IMessageHandler));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-required", messageHandler.GetMessage(IdMessage).Name);
        }

        private void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if(attributes.ContainsKey(key))
            {
                return;
            }

            attributes.Add(key, value);
        }
    }
}