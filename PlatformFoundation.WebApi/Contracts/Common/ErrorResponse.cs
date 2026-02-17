namespace PlatformFoundation.WebApi.Contracts.Common;

public sealed record ErrorResponse(
    string TraceId,
    int Status,
    string Title,
    string? Detail = null,
    IReadOnlyDictionary<string, string[]>? Errors = null);