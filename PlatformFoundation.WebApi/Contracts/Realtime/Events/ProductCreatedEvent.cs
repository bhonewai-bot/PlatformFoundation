namespace PlatformFoundation.WebApi.Contracts.Realtime.Events;

public sealed record ProductCreatedEvent(
    Guid Id,
    string Name,
    decimal Price);
