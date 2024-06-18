using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Shared.Common.Constants;

public static class ClaimConstant
{
    public const string IsAdminClaim = "IsAdmin";
    public const string Sub = "sub";
    public const string EmailClaim = "email";
    public const string IsEmailVerifiedClaim = "email_verified";
    public const string AwsRoleClaim = "custom:role";
    public const string AwsUserNameClaim = "cognito:username";
}
