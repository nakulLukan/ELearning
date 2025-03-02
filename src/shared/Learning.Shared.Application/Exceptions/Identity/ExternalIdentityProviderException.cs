namespace Learning.Shared.Application.Exceptions.Identity;

public class ExternalIdentityProviderException : Exception
{
    public ExternalIdentityProviderExceptionType Type { get; private set; }
    public ExternalIdentityProviderException(ExternalIdentityProviderExceptionType type) : base()
    {
        Type = type;
    }
}

public enum ExternalIdentityProviderExceptionType
{
    IncorrectCredentials,
    AccountNotFound,
    AccountNotConfirmed,
    UserAlreadyExists
}
