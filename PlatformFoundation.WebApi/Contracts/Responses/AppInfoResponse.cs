namespace PlatformFoundation.WebApi.Contracts.Responses;

public sealed record AppInfoResponse(
    string AppName,
    string Environment,
    string Version,
    DateTime UtcNow);
