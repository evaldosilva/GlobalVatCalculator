using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GlobalVatCalculator.API.Handlers;

public class ProblemExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<ProblemExceptionHandler> logger) : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;
    private readonly ILogger<ProblemExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not OverflowException handledException)
        {
            _logger.LogError("Unexpected handled exception ocurried. Details: {@exception}", exception);
            return true;
        }

        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Calculation result error",
            Detail = handledException.Message,
            Type = "Bad Request: https://tools.ietf.org/html/rfc9110#section-15.5.1"
        };

        _logger.LogWarning("Handled exception ocurried. Details: {exceptionMessage}", exception.Message);

        return await _problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails
                }
            );
    }
}