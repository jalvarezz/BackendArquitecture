using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Common.Extensions
{
    /// <summary>
    /// Summary description for String Extensions
    /// </summary>
    public static class StringExtensions
    {
        public static string ToShortCreditCard(this string str)
        {
            Regex regex = new Regex(@"((?:(?:4\d{3})|(?:5[1-5]\d{2})|6(?:011|5[0-9]{2}))(?:-?|\040?)(?:\d{4}(?:-?|\040?)){3}|(?:3[4,7]\d{2})(?:-?|\040?)\d{6}(?:-?|\040?)\d{5})");

            if (regex.Match(str).Success)
            {
                return str.Substring(0, 4) + "-****-****-" + str.Substring(str.Length - 4, 4);
            }
            else
            {
                return str;
            }
        }

        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        public static IEnumerable<string> SplitByLenght(this string str, int maxLength)
        {
            int index = 0;
            while (true)
            {
                if (index + maxLength >= str.Length)
                {
                    yield return str.Substring(index);
                    yield break;
                }
                yield return str.Substring(index, maxLength);
                index += maxLength;
            }
        }

        public static List<T> Split<T>(this string value, string separator)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new List<T>();

            return value.Split(separator)
                        .Select(x => (T)Convert.ChangeType(x, typeof(T)))
                        .ToList();
        }

        public static string FormatSSN(this string ssn)
        {
            if (ssn.Length == 9)
            {
                string lastDigits = ssn.Substring(ssn.Length - 4, 4);

                return string.Format("*****{0}", lastDigits);
            }

            return ssn;
        }

        public static string CalculateMD5Hash(this string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GetLast(this string str, int tail_length)
        {
            if (tail_length >= str.Length)
                return str;
            return str.Substring(str.Length - tail_length);
        }

        public static string RemoveSpecialChars(this string str)
        {
            char[] buffer = new char[str.Length];
            int idx = 0;

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z')
                    || (c >= 'a' && c <= 'z'))
                {
                    buffer[idx] = c;
                    idx++;
                }
            }

            return new string(buffer, 0, idx);
        }

        public static string ReplaceSpecialChars(this string str)
        {
            return Regex.Replace(str.Normalize(NormalizationForm.FormD), "[^0-9a-zA-Z:,-]+", string.Empty);
        }

        /// <summary>
        /// Insert spaces before capital letter in the string. I.e. "HelloWorld" turns into "Hello World"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSeparatedWords(this string value)
        {
            if (value != null)
            {
                return Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim();
            }
            return null;
        }

        /// <summary>
        ///     Removes dashes ("-") from the given object value represented as a string and returns an empty string ("")
        ///     when the instance type could not be represented as a string.
        ///     <para>
        ///         Note: This will return the type name of given isntance if the runtime type of the given isntance is not a
        ///         string!
        ///     </para>
        /// </summary>
        /// <param name="value">The object instance to undash when represented as its string value.</param>
        /// <returns></returns>
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        /// <summary>
        ///     Removes dashes ("-") from the given string value.
        /// </summary>
        /// <param name="value">The string value that optionally contains dasNotificationSender.</param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }
    }
}
