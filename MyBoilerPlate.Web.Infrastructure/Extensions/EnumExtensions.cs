using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBoilerPlate.Web.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Given an enum value, produce a human-readable interpretation of enum.
        /// First looks into DisplayAttribute - returns that if the value is decorated.
        /// If not, returns the text representation of the value, but with spaces between words.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            if(value == null)
            {
                return string.Empty;
            }

            var field = value.GetType().GetField(value.ToString());

            if(field == null)
            {
                return string.Empty;
            }

            // first try Display Attribute on the enum
            var attribs = field.GetCustomAttributes(typeof(DisplayAttribute), true);
            if(attribs.Length > 0)
            {
                return ((DisplayAttribute)attribs[0]).GetName();
            }

            // if Display attribute is not present, try description
            attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if(attribs.Length > 0)
            {
                return ((DescriptionAttribute)attribs[0]).Description;
            }

            // if none of the above attributes are present, just put spaces where needed on the name
            return value.ToString().ToSeparatedWords();
        }
    }
}