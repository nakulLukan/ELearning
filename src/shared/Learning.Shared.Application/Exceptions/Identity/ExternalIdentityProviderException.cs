namespace Learning.Shared.Application.Exceptions.Identity;

public class ExternalIdentityProviderException : Exception
{
    public string? Message { get; set; }
    public ExternalIdentityProviderExceptionType Type { get; private set; }
    public ExternalIdentityProviderException(ExternalIdentityProviderExceptionType type) : base()
    {
        Type = type;
    }
    public ExternalIdentityProviderException(ExternalIdentityProviderExceptionType type, string message) : this(type)
    {
        Message = message;
    }
}

public enum ExternalIdentityProviderExceptionType
{
    NotAuthorized,
    IncorrectCredentials,
    PasswordAttemptExceeds,
    AccountNotFound,
    AccountNotConfirmed,
    UserAlreadyExists
}
