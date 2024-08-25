using System.Text;

namespace Learning.Shared.Common.Utilities;

public static class CouponCodeGenerator
{
    public static string GenerateCouponCode(int groupLength = 4, int groupCount = 4)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < groupCount; i++)
        {
            if (i > 0)
            {
                stringBuilder.Append('-');
            }

            for (int j = 0; j < groupLength; j++)
            {
                stringBuilder.Append(chars[Random.Shared.Next(chars.Length)]);
            }
        }

        return stringBuilder.ToString();
    }
}
