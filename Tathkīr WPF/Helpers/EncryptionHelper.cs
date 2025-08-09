using System.Security.Cryptography;
using System.Text;

namespace Tathkīr_WPF.Helpers
{
    public static class EncryptionHelper
    {
        public static byte[] Encrypt(string plainText)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            return ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
        }

        public static string Decrypt(byte[] encryptedData)
        {
            var bytes = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
