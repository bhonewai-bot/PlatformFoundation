namespace PlatformFoundation.WebApi.Contracts.Common;

public sealed record AppInfoResponse(
    string AppName,
    string Environment,
    string Version,
    DateTime UtcNow);
