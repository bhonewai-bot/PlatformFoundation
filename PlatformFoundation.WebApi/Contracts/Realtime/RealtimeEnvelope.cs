namespace PlatformFoundation.WebApi.Contracts.Realtime;

public sealed record RealtimeEnvelope<T>(
    string Type,
    int Version,
    string MessageId,
    DateTime UtcNow,
    T Data);
