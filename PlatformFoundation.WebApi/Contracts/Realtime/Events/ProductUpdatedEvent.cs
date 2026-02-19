namespace PlatformFoundation.WebApi.Contracts.Realtime.Events;

public sealed record ProductUpdatedEvent(Guid Id, string Name, decimal Price);
