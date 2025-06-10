using Learning.Shared.Constants;
using System.Security.Cryptography;

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
        var otpString = Math.Abs(otp).ToString("D6").ToCharArray();
        
        for (int i = 0; i < otpString.Length; i++)
        {
            if (otpString[i] == '0')
            {
                otpString[i] = '1';
            }
            else
            {
                return int.Parse(new string(otpString));
            }
        }
        return otp;
    }
}
