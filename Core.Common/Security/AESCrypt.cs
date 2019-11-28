using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Core.Common.Security
{
    public class AESCrypt
    {
        #region Private Fields
        private byte[] Key;
        private byte[] IV;
        #endregion Private Fields

        #region Read-Only Properties
        public String Key_Str
        {
            get
            {
                return Convert.ToBase64String(Key);
            }
        }

        public String IV_Str
        {
            get
            {
                return Convert.ToBase64String(IV);
            }
        }
        #endregion Read-Only Properties

        #region Constructors

        public AESCrypt()
        {
            RijndaelManaged AES_Alg = null;

            try
            {
                // Tip: When the object is created, Key and IV properties are initialized to random values.
                //      If the class is created using this constructors, we will use the random values.etc for that. 
                // Tip: Default Cipher Mode is: CipherMode.CBC
                AES_Alg = new RijndaelManaged();
                Key = AES_Alg.Key;
                IV = AES_Alg.IV;
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (AES_Alg != null)
                    AES_Alg.Clear();
            }
        }

        public AESCrypt(String pass, String keySalt)
			: this( pass, Encoding.ASCII.GetBytes( keySalt ) )
		{

        }

        public AESCrypt(String pass, byte[] keySalt)
        {

            if (pass == null || pass.Length <= 0)
                throw new ArgumentNullException("pass");
            if (keySalt == null || keySalt.Length <= 0)
                throw new ArgumentNullException("keySalt");

            // Derive a Key and an IV from the Password and create an algorithm
            //PasswordDeriveBytes pdb = new PasswordDeriveBytes( pass, keySalt ); // Weak - based on PBKDF1
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pass, keySalt); // Strong - based on PBKDF2

            Key = pdb.GetBytes(32);
            IV = pdb.GetBytes(16);
        }
        #endregion Constructor

        #region Methods
        public String Encrypt(String str)
        {
            // Check arguments.
            if (str == null || str.Length <= 0)
                throw new ArgumentNullException("str");

            RijndaelManaged AES_Alg = null;

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;

            try
            {
                // Tip: When the object is created, Key and IV properties are initialized to random values.
                //      In this class we want fixed Key and IV values. We use a PasswordDeriveBytes objetc for that. 
                // Tip: Default Cipher Mode is: CipherMode.CBC
                AES_Alg = new RijndaelManaged();
                AES_Alg.Key = Key;
                AES_Alg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = AES_Alg.CreateEncryptor(AES_Alg.Key, AES_Alg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();

                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(str);
                    }
                }

            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (AES_Alg != null)
                    AES_Alg.Clear();
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public String Decrypt(String str)
        {

            byte[] cipherText = Convert.FromBase64String(str);

            // Check arguments.
            if (str == null || str.Length <= 0)
                throw new ArgumentNullException("str");

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged AES_Alg = null;

            // Declare the string used to hold
            // the decrypted text.
            String plaintext = null;

            try
            {
                // Tip: When the object is created, Key and IV properties are initialized to random values.
                //      In this class we want fixed Key and IV values. We use a PasswordDeriveBytes objetc for that. 
                AES_Alg = new RijndaelManaged();
                AES_Alg.Key = Key;
                AES_Alg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = AES_Alg.CreateDecryptor(AES_Alg.Key, AES_Alg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }


            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (AES_Alg != null)
                    AES_Alg.Clear();
            }

            return plaintext;
        }

        public void GenerateNewValues(String pass, String keySalt)
        {
            GenerateNewValues(pass, Encoding.ASCII.GetBytes(keySalt));
        }

        public void GenerateNewValues(String pass, byte[] keySalt)
        {

            if (pass == null || pass.Length <= 0)
                throw new ArgumentNullException("pass");
            if (keySalt == null || keySalt.Length <= 0)
                throw new ArgumentNullException("keySalt");

            // Derive a Key and an IV from the Password and create an algorithm
            //PasswordDeriveBytes pdb = new PasswordDeriveBytes( pass, keySalt ); // Weak - based on PBKDF1
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pass, keySalt); // Strong - based on PBKDF2

            Key = pdb.GetBytes(32);
            IV = pdb.GetBytes(16);
        }


        public static String GetPasswordDerivedBytes(String PassPhrase, String SaltValue, int Size)
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(PassPhrase, Encoding.ASCII.GetBytes(SaltValue));

            return Convert.ToBase64String(pdb.GetBytes(Size));
        }




        #endregion Methods
    }
}
