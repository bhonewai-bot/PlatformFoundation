using Microsoft.AspNetCore.SignalR;
using PlatformFoundation.WebApi.Contracts.Realtime;

namespace PlatformFoundation.WebApi.Hubs;

public sealed class NotificationsHub : Hub
{
    private readonly ILogger<NotificationsHub> _logger;

    public NotificationsHub(ILogger<NotificationsHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("SignalR connected: connectionId={ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception is null)
            _logger.LogInformation("SignalR disconnected: connectionId={ConnectionId}", Context.ConnectionId);
        else 
            _logger.LogError("SignalR disconnected with error: connectionId={ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public Task SendPing(string message)
    {
        var env = new RealtimeEnvelope<PingRealtimeEvent>(
            Type: "ping",
            Version: 1,
            MessageId: Guid.NewGuid().ToString(),
            UtcNow: DateTime.UtcNow,
            Data: new PingRealtimeEvent(message));
        
        return Clients.Caller.SendAsync("ping", env);
    }
}
