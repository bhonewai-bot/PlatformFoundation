namespace PlatformFoundation.WebApi.Realtime;

public static class GroupNames
{
    public static string User(string userId)
        => $"user:{userId.Trim()}";

    public static string Topic(string topic)
        => $"topic:{NormalizeTopic(topic)}";

    private static string NormalizeTopic(string topic)
        => topic.Trim().ToLowerInvariant().Replace(' ', '-');
}
