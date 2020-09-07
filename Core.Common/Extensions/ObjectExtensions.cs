using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Core.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJSON(this object obj)
        {
            string result = string.Empty;

            //Write the JSON
            MemoryStream strm = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(strm, obj);
            string jsonString = Encoding.Default.GetString(strm.ToArray());

            result = jsonString;

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
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Performs a deep Copy of an object that has a datacontract
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneWithDataContract<T>(this T source)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(T));
                dcs.WriteObject(stream, source);
                stream.Position = 0;
                return (T)dcs.ReadObject(stream);
            }
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

        public static byte[] SerializeToBytes(this object obj)
        {
            var formatter = new BinaryFormatter();
            using(var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }

        public static T DeserializeFromBytes<T>(this byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using(var stream = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(stream);
            }
        }

        public static byte[] CalculateMD5Hash(this object obj)
        {
            StringBuilder sb = new StringBuilder();

            var properties = obj.GetType().GetProperties();

            foreach(var property in properties)
            {
                var propValue = property.GetValue(obj);

                if(propValue != null)
                    sb.Append(propValue.ToString());
            }

            var ctValues = sb.ToString();

            if(string.IsNullOrEmpty(ctValues))
                return null;

            // step 1, calculate MD5 hash from concatenated values
            MD5 md5 = MD5.Create();
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
            if(obj != null)
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
