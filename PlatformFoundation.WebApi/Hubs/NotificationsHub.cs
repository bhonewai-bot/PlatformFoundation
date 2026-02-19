using Microsoft.AspNetCore.SignalR;
using PlatformFoundation.WebApi.Contracts.Realtime;
using PlatformFoundation.WebApi.Realtime;

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

    public async Task JoinUser(string userId)
    {
        var group = GroupNames.User(userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, group, Context.ConnectionAborted);
        
        _logger.LogInformation("Joined group: connectionId={ConnectionId} group={Group}", Context.ConnectionId, group);

        await Clients.Caller.SendAsync("system", new { message = $"joined {group}" });
    }

    public async Task LeaveUser(string userId)
    { 
        var group = GroupNames.User(userId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group, Context.ConnectionAborted);
        
        _logger.LogInformation("Left group: connectionId={ConnectionId} group={Group}", Context.ConnectionId, group);

        await Clients.Caller.SendAsync("system", new { message = $"left {group}" });
    }

    public async Task JoinTopic(string topic)
    {
        var group = GroupNames.Topic(topic);
        await Groups.AddToGroupAsync(Context.ConnectionId, group, Context.ConnectionAborted);
        
        _logger.LogInformation("Joined topic: connectionId={ConnectionId} group={Group}", Context.ConnectionId, group);

        await Clients.Caller.SendAsync("system", new { message = $"joined {group}" });
    }

    public async Task LeaveTopic(string topic)
    {
        var group = GroupNames.Topic(topic);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group, Context.ConnectionAborted);
        
        _logger.LogInformation("Left topic: connectionId={ConnectionId} group={Group}", Context.ConnectionId, group);
        
        await Clients.Caller.SendAsync("system", new { message = $"left {group}" });
    }

    public Task SendToUser(string userId, string message)
    {
        var env = new RealtimeEnvelope<PingRealtimeEvent>(
            Type: "ping",
            Version: 1,
            MessageId: Guid.NewGuid().ToString(),
            UtcNow: DateTime.UtcNow, 
            Data: new PingRealtimeEvent(message));

        return Clients.Group(GroupNames.User(userId)).SendAsync("realtime", env);
    }

    public Task SendToTopic(string topic, string message)
    {
        var env = new RealtimeEnvelope<PingRealtimeEvent>(
            Type: "ping",
            Version: 1,
            MessageId: Guid.NewGuid().ToString(),
            UtcNow: DateTime.UtcNow, 
            Data: new PingRealtimeEvent(message));

        return Clients.Group(GroupNames.Topic(topic)).SendAsync("realtime", env);
    }
    
    public Task SendPing(string message)
    {
        var env = new RealtimeEnvelope<PingRealtimeEvent>(
            Type: "ping",
            Version: 1,
            MessageId: Guid.NewGuid().ToString(),
            UtcNow: DateTime.UtcNow,
            Data: new PingRealtimeEvent(message));

        return Clients.Caller.SendAsync("realtime", env);
    }
}
