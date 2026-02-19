using Microsoft.AspNetCore.SignalR;
using PlatformFoundation.WebApi.Contracts.Realtime;
using PlatformFoundation.WebApi.Contracts.Realtime.Events;
using PlatformFoundation.WebApi.Hubs;

namespace PlatformFoundation.WebApi.Realtime;

public sealed class RealtimePublisher : IRealtimePublisher
{
    private readonly IHubContext<NotificationsHub> _hub;
    private readonly ILogger<RealtimePublisher> _logger;

    public RealtimePublisher(IHubContext<NotificationsHub> hub, ILogger<RealtimePublisher> logger)
    {
        _hub = hub;
        _logger = logger;
    }

    public async Task PublishToTopic<T>(string topic, string type, int version, T data, CancellationToken ct)
    {
        var group = GroupNames.Topic(topic);

        var env = new RealtimeEnvelope<T>(
            Type: type,
            Version: version,
            MessageId: Guid.NewGuid().ToString(),
            UtcNow: DateTime.UtcNow,
            Data: data);
        
        _logger.LogInformation("Publishing realtime event: type={Type}, group={Group}", type, group);

        await _hub.Clients.Group(group).SendAsync("realtime", env, ct);
    }
}
