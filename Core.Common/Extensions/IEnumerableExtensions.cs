using System;
using System.Collections.Generic;

namespace Core.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool TryParseToCollection<T>(this string model,
            out ICollection<T> enumerable) where T : struct
        {
            var result = false;
            enumerable = new List<T>();

            foreach(string number in model.Split(','))
            {
                result = TryParse(number, out T parsed);

                if(!result)
                    break;

                enumerable.Add(parsed);
            }

            return result;
        }

        public static bool TryParse<T>(string model, out T parsed) where T : struct
        {
            parsed = default;
            var result = false;

            if(ValidateType<int>() && (result = int.TryParse(model, out int intOutput)))
                parsed = ConvertType(intOutput, typeof(T));

            if(ValidateType<decimal>() && (result = decimal.TryParse(model, out decimal decimalOutput)))
                parsed = ConvertType(decimalOutput, typeof(T));

            if(ValidateType<DateTime>() && (result = DateTime.TryParse(model, out DateTime dateOutput)))
                parsed = ConvertType(dateOutput, typeof(T));

            if(ValidateType<byte>() && (result = byte.TryParse(model, out byte byteOutput)))
                parsed = ConvertType(byteOutput, typeof(T));

            if(ValidateType<float>() && (result = float.TryParse(model, out float floatOutput)))
                parsed = ConvertType(floatOutput, typeof(T));

            if(ValidateType<double>() && (result = double.TryParse(model, out double doubleOutput)))
                parsed = ConvertType(doubleOutput, typeof(T));

            return result;

            T ConvertType(object toaConvert, Type type) => (T)Convert.ChangeType(toaConvert, type);

            bool ValidateType<TOut>() => typeof(T) == typeof(TOut);
        }
    }
}
