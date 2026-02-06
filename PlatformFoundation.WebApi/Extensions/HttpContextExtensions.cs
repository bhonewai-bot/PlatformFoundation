using PlatformFoundation.WebApi.Middlewares;

namespace PlatformFoundation.WebApi.Extensions;

public static class HttpContextExtensions
{
    public static string GetCorrelationId(this HttpContext context)
    {
        return context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var cidObj) &&
               cidObj is string cid && !string.IsNullOrWhiteSpace(cid)
            ? cid
            : context.TraceIdentifier;
    }
}