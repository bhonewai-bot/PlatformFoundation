using System.Text.Json;
using PlatformFoundation.Domain.Exceptions;
using PlatformFoundation.WebApi.Contracts.Responses;
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
            _logger.LogError(ex, "Unhandled exception.");
            
            await WriteErrorResponse(context, ex);
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, Exception ex)
    {
        var traceId = context.GetCorrelationId();

        var (status, title, detail) = ex switch
        {
            DomainValidationException dve => (StatusCodes.Status400BadRequest, "Validation failed", dve.Message),
            DomainException de => (StatusCodes.Status400BadRequest, "Domain error", de.Message),

            _ => (StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occured.")
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;
        
        var payload = new ErrorResponse(
            TraceId: traceId,
            Status: status,
            Title: title,
            Detail: detail);

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}