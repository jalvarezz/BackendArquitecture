using System;
using Core.Common.Security;
using Core.Common.Settings;
using System.Text;
using System.Security.Cryptography;
using Core.Common.Contracts;

namespace MyBoilerPlate.Web.Services
{
    public class CryptProvider : ICryptProvider
    {
        private string _OriginalKey;
        private string _OriginalSalt;

        public CryptProvider(AESSecuritySettings securitySetting)
        {
            _OriginalKey = securitySetting.Key;
            _OriginalSalt = securitySetting.Salt;
        }

        public string Decrypt(string str)
        {
            AESCrypt crypt = new AESCrypt(_OriginalKey, _OriginalSalt);

            return crypt.Decrypt(str);
        }

        public string Decrypt(string str, string key, string salt)
        {
            //The private properties of _Crypt where not refreshing, needed to create new instance
            AESCrypt enc = new AESCrypt(key, salt);

            return enc.Decrypt(str);
        }

        public string DecryptWithEncode(string str)
        {
            AESCrypt crypt = new AESCrypt(_OriginalKey, _OriginalSalt);

            return crypt.Decrypt(System.Web.HttpUtility.UrlDecode(str));
        }

        public T DecryptWithEncode<T>(string str)
        {
            AESCrypt crypt = new AESCrypt(_OriginalKey, _OriginalSalt);

            return (T)Convert.ChangeType(System.Web.HttpUtility.UrlDecode(crypt.Decrypt(str.Replace(",", "/").Replace(".", "+").Replace("=", "=="))), typeof(T));
        }

        public string Encrypt(string str)
        {
            AESCrypt crypt = new AESCrypt(_OriginalKey, _OriginalSalt);

            return crypt.Encrypt(str);
        }

        public string Encrypt(string str, string key, string salt)
        {
            AESCrypt enc = new AESCrypt(key, salt);

            return enc.Encrypt(str);
        }

        public string EncryptWithEncode(string str)
        {
            AESCrypt crypt = new AESCrypt(_OriginalKey, _OriginalSalt);

            return System.Web.HttpUtility.UrlDecode(crypt.Encrypt(str).Replace("/", ",").Replace("+", "%2E").Replace("==", "="));
        }
    }
}
