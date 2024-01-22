using System.Net;

namespace Learning.Shared.Common.Utilities;
public class AppApiException : Exception
{
    public string ErrorMessage { get; }
    public HttpStatusCode StatusCode { get; }
    public string ErrorCode { get; set; }

    public AppApiException(HttpStatusCode statusCode, string errorCode, string errorMessage) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}
