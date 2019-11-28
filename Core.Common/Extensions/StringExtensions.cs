using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography;

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

            if(regex.Match(str).Success)
            {
                return str.Substring(0,4) + "-****-****-" + str.Substring(str.Length-4,4);
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
    }
}
