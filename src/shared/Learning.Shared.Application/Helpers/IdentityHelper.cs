namespace Learning.Shared.Application.Helpers;

public class IdentityHelper
{
    public static bool IsAdminUser(string? role) => !string.IsNullOrEmpty(role);
}
