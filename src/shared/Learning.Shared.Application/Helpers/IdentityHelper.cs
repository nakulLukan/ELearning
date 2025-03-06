using System.Security.Cryptography;
using Learning.Shared.Constants;

namespace Learning.Shared.Application.Helpers;

public class IdentityHelper
{
    public static bool IsAdminUser(string? role) => !string.IsNullOrEmpty(role);
    public static string ToMobileNumber(string username) => username.StartsWith(LocalizationConstant.CountryCode) ? username : $"{LocalizationConstant.CountryCode}{username}";

    /// <summary>
    /// Changes given number to 10 digit number without country code
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public static string ToUsername(string phoneNumber) => !phoneNumber.StartsWith(LocalizationConstant.CountryCode) ? phoneNumber : phoneNumber.Substring(LocalizationConstant.CountryCode.Length, (phoneNumber.Length - LocalizationConstant.CountryCode.Length));

    public static int GenerateOtp()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        int otp = BitConverter.ToInt32(bytes, 0) % 1000000;
        var otpString = Math.Abs(otp).ToString("D6");
        return int.Parse(otpString.Length == 6 ? otpString : MakeIt6Digit(otpString)); // Ensures 6-digit format
    }

    private static string MakeIt6Digit(string otp)
    {
        int len = (6 - otp.Length);
        for (int i = 0; i < len; i++)
        {
            otp += i;
        }

        return otp;
    }
}
