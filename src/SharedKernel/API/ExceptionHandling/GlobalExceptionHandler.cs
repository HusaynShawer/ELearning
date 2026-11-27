using ELearning.SharedKernel.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace ELearning.SharedKernel.API.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode;

        if (exception is AppException appException)
        {
            statusCode = appException.StatusCode;
            _logger.LogWarning(
                appException,
                "Application exception occurred: {Message}",
                appException.Message);
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            _logger.LogError(exception, "An unhandled exception occurred.");
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            StatusCode = statusCode,
            Message = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : exception.Message
        }, cancellationToken);

        return true;
    }
}
