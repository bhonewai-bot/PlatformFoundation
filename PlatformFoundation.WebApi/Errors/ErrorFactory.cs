using PlatformFoundation.WebApi.Contracts.Common;
using PlatformFoundation.WebApi.Contracts.Products.Responses;

namespace PlatformFoundation.WebApi.Errors;

public static class ErrorFactory
{
    public static ErrorResponse ValidationFailed(string traceId, string detail, IReadOnlyDictionary<string, string[]>? errors = null)
        => new(traceId, StatusCodes.Status400BadRequest, "Validation failed", detail, errors);
    
    public static ErrorResponse NotFound(string traceId, string detail = "Resource not found.")
        => new(traceId, StatusCodes.Status404NotFound, "Not found", detail, null);

    public static ErrorResponse Conflict(string traceId, string detail = "Conflict.")
        => new(traceId, StatusCodes.Status409Conflict, "Conflict", detail, null);
    
    public static ErrorResponse TooManyRequests(string traceId, string detail = "Rate limit exceeded. Please try again later.")
        => new(traceId, StatusCodes.Status429TooManyRequests, "Too Many Requests", detail, null);
    
    public static ErrorResponse ServerError(string traceId)
        => new(traceId, StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.", null);
}
