using MyBoilerPlate.Business.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MyBoilerPlate.Web.Infrastructure.Attributes
{


    public sealed class RequiredIfAttribute : ValidationAttribute
    {
        readonly RequiredAttribute _InnerAttribute = new RequiredAttribute();
        public string DependentProperty { get; set; }
        public string PrpName { get; set; }
        public string IdMessage { get; set; }
        public RequiredIfAttribute(string dependentProperty, string prpName, string IdMessage)

        {
            PrpName = prpName;
            DependentProperty = dependentProperty;
            this.IdMessage = IdMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = validationContext.ObjectType.GetProperty(DependentProperty);

            var messageHandler = (IMessageHandler)validationContext.GetService(typeof(IMessageHandler));

            if(field != null)
            {
                var IsOk = false;
                if(_InnerAttribute.IsValid(value))
                {
                    IsOk = true;
                }

                return (IsOk) ? ValidationResult.Success : new ValidationResult(messageHandler.GetMessage(IdMessage).Name);
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(DependentProperty));
            }
        }
    }

}
