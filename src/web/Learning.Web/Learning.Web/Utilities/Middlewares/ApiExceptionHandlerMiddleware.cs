using System.Net;
using System.Text.Json;
using Learning.Shared.Common.Utilities;
using Refit;

namespace Learning.Web.Utilities.ExceptionHandler;

public class ApiExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionHandlerMiddleware> _logger;

    public ApiExceptionHandlerMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred.");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string message;

        if (exception is AppApiException apiException)
        {
            statusCode = apiException.StatusCode;
            message = apiException.ErrorCode;
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = "An unexpected error occurred.";
        }

        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred",
            Status = (int)statusCode,
            Detail = message,
            Instance = context.Request.Path,
        };
        var jsonResponse = JsonSerializer.Serialize(problemDetails);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(jsonResponse);
    }
}
