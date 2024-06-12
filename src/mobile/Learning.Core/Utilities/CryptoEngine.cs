using System.Security.Cryptography;
using System.Text;

namespace Learning.Core.Utilities;

public static class CryptoEngine
{
    public static string DecryptViaAes(string inputText, string secret)
    {
        using Aes aes = Aes.Create();

        byte[] encryptedData = Convert.FromBase64String(inputText);
        byte[] salt = Encoding.ASCII.GetBytes(secret.Length.ToString());
        using PasswordDeriveBytes secretKey = new(secret, salt);

        // Set AES key and IV
        aes.Key = secretKey.GetBytes(32);
        aes.IV = secretKey.GetBytes(16);

        // Create a decryptor from the existing AES instance.
        using ICryptoTransform decryptor = aes.CreateDecryptor();

        using MemoryStream memoryStream = new(encryptedData);

        // Create a CryptoStream. (always use Read mode for decryption).
        using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);

        // Since we don't know the exact length of decrypted data, let's use a MemoryStream to hold it.
        using MemoryStream decryptedMemoryStream = new();

        int bytesRead;
        byte[] buffer = new byte[1024]; // Buffer to read decrypted data.

        // Read decrypted data from CryptoStream and write it to decryptedMemoryStream.
        while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            decryptedMemoryStream.Write(buffer, 0, bytesRead);
        }

        // Convert decryptedMemoryStream to a byte array.
        byte[] decryptedData = decryptedMemoryStream.ToArray();

        // Convert decrypted data into a string.
        string decryptedString = Encoding.Unicode.GetString(decryptedData);

        // Return decrypted string.
        return decryptedString;
    }
}
