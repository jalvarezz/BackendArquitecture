using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Core.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJSON(this object obj)
        {
            string result = JsonSerializer.Serialize(obj);

            return result;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (source is null)
            {
                return default;
            }

            // Serialize to JSON and deserialize to clone
            var jsonObject = JsonSerializer.Serialize(source);
            var result = JsonSerializer.Deserialize<T>(jsonObject);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        public static T DeserializeFromBytes<T>(this byte[] bytes)
        {
            var formatter = new BinaryFormatter();

            using var stream = new MemoryStream(bytes);
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            return (T)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        }

        public static byte[] CalculateMD5Hash(this object obj)
        {
            var sb = new StringBuilder();

            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propValue = property.GetValue(obj);

                if (propValue != null)
                    sb.Append(propValue.ToString());
            }

            var ctValues = sb.ToString();

            if (string.IsNullOrEmpty(ctValues))
                return null;

            // step 1, calculate MD5 hash from concatenated values
            var md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(ctValues);
            byte[] hash = md5.ComputeHash(inputBytes);

            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyNames"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string PropertyValuesAsString(this object obj, string[] propertyNames, string separator = "=")
        {
            if (obj != null)
            {
                var values = obj.GetType().GetProperties()
                    .Where(x => propertyNames.Contains(x.Name))
                        .Select(x =>
                        new
                        {
                            property = x.Name,
                            value = x.GetValue(obj, null),
                        }
                        ).ToDictionary(x => x.property, y => y.value);

                return string.Join(";", values.Select(x => x.Key + separator + x.Value).ToArray());
            }
            else
            {
                return "NULL";
            }
        }
    }
}
