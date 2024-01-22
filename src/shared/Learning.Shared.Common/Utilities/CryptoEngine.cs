using System.Security.Cryptography;
using System.Text;

namespace Learning.Shared.Common.Utilities;

public static class CryptoEngine
{
    public static byte[] EncryptText(string secretMessage, string secretKey)
    {
        using (Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(secretKey, new byte[16], 995, HashAlgorithmName.SHA256))
        {
            var key = derivedBytes.GetBytes(256 / 8);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.Mode = CipherMode.ECB;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor();

                byte[] plaintextBytes = Encoding.UTF8.GetBytes(secretMessage);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

                return encryptedBytes;
            }
        }
    }

    public static string DecryptText(this byte[] encryptedText, string secretKey)
    {
        try
        {
            using (Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(secretKey, new byte[16], 995, HashAlgorithmName.SHA256))
            {
                var key = derivedBytes.GetBytes(256 / 8);
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.Mode = CipherMode.ECB;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor();

                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length);
                    string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                    return decryptedText;
                }
            }
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string DecryptText(this string cipherText, string secretKey)
    {
        var encryptedText = Convert.FromBase64String(cipherText);
        return DecryptText(encryptedText, secretKey);
    }
}
