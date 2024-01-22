namespace Learning.Shared.Common.Utilities;
public class AppException : Exception
{
    public string ErrorMessage { get; }
    public bool IsInAccessible { get; set; }

    public AppException(string errorMessage, bool isInAccessible = false) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
        IsInAccessible = isInAccessible;
    }
}
