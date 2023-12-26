using System.Security.Cryptography;
using System.Text;

namespace Learning.Shared.Common.Extensions;

public static class HashExtensions
{
    public static string Sha256(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        using SHA256 sHA = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(sHA.ComputeHash(bytes));
    }
}
