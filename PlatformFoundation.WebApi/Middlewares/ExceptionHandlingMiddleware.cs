using PlatformFoundation.Domain.Exceptions;
using PlatformFoundation.WebApi.Errors;
using PlatformFoundation.WebApi.Extensions;

namespace PlatformFoundation.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is DomainValidationException || ex is DomainConflictException)
            {
                _logger.LogWarning("Domain validation failed: {Message}", ex.Message);
            }
            else
            {
                _logger.LogError(ex, "Unhandled exception.");
            }
            
            await WriteErrorResponse(context, ex);
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, Exception ex)
    {
        var traceId = context.GetCorrelationId();

        var (status, payload) = ex switch
        {
            DomainValidationException dve => (400, ErrorFactory.ValidationFailed(traceId, dve.Message)),
            DomainConflictException dce => (409, ErrorFactory.Conflict(traceId, dce.Message)),
            DomainException de => (400, ErrorFactory.ValidationFailed(traceId, de.Message)),
            _ => (500, ErrorFactory.ServerError(traceId))
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        await context.Response.WriteAsJsonAsync(payload);
    }
}
