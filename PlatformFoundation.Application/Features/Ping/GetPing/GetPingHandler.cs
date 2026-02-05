namespace PlatformFoundation.Application.Features.Ping.GetPing;

public sealed class GetPingHandler
{
    public Task<PingResult> Handle(GetPingQuery query, CancellationToken ct)
    {
        var result = new PingResult("pong", DateTime.UtcNow);
        return Task.FromResult(result);
    }
}