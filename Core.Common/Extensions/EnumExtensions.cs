using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Core.Common.Extensions
{
    public static class EnumExtensions
    {
        public static T GetValueFromDescription<T>(this Enum value, string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
        }

        public static int GetEnumFromDescription(this Type enumType, string description)
        {
            foreach (var field in enumType.GetFields())
            {
                DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null)
                    continue;
                if (attribute.Description == description)
                {
                    return (int)field.GetValue(null);
                }
            }
            return 0;
        }
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));

                        if (memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Given an enum value, produce a human-readable interpretation of enum.
        /// First looks into DisplayAttribute - returns that if the value is decorated.
        /// If not, returns the text representation of the value, but with spaces between words.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var field = value.GetType().GetField(value.ToString());

            if (field == null)
            {
                return string.Empty;
            }

            // first try Display Attribute on the enum
            var attribs = field.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (attribs.Length > 0)
            {
                return ((DisplayAttribute)attribs[0]).GetName();
            }

            // if Display attribute is not present, try description
            attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attribs.Length > 0)
            {
                return ((DescriptionAttribute)attribs[0]).Description;
            }

            // if none of the above attributes are present, just put spaces where needed on the name
            return value.ToString().ToSeparatedWords();
        }
    }
}
