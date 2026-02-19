namespace PlatformFoundation.WebApi.Realtime;

public interface IRealtimePublisher
{
    Task PublishToTopic<T>(string topic, string type, int version, T data, CancellationToken ct);
}
