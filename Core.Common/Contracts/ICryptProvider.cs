namespace Core.Common.Contracts
{
    public interface ICryptProvider
    {
        string Encrypt(string str);

        string Encrypt(string str, string key, string salt);

        string Decrypt(string str);

        string Decrypt(string str, string key, string salt);

        string EncryptWithEncode(string str);

        string DecryptWithEncode(string str);

        T DecryptWithEncode<T>(string str);

        bool MembershipProviderCheckPassword(string password, string salt, string hashedPassword);

        string MembershipEncodePassword(string pass, string salt, string hashName);
        string MembershipGeneratePaswordSalt(int size);

    }
}
