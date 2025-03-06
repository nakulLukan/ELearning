namespace Learning.Web.Client.Constants;

public static class PolicyConstant
{
    public const string AdminPolicy = "admin";
    public const string CouponCodePolicy = "coupon-code";
    public const string ContactUsRequestPolicy = "contact-us-request";
    public const string UserAccountPolicy = "user-accounts";
    public const string ExamNotificationPolicy = "exam-notification";
    public const string QuizPolicy = "quiz";
}

public static class RateLimitingPolicyConstant
{
    public const string SignupPage = "SignupPage";
    public const string ConfirmAccountPage = "VerifyAccountPage";
    public const string ForgotPasswordPage = "ForgotPasswordPage";
}