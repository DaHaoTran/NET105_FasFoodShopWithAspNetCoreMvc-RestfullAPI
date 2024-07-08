using System.Security.Cryptography;
using System.Text;

namespace API.Services.Implement
{
    public static class AuthencationDataSvc
    {
        public static string GenerateNewCode(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] codeChars = new char[length];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomData = new byte[length];
                rng.GetBytes(randomData);

                for (int i = 0; i < codeChars.Length; i++)
                {
                    int pos = randomData[i] % validChars.Length;
                    codeChars[i] = validChars[pos];
                }
            }
            return new string(codeChars);
        }
        public static string EncryptionPassword(string password)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = md5.ComputeHash(encoding.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }
    }
}
