using Serilog.Context;

namespace PlatformFoundation.WebApi.Middlewares;

public sealed class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-ID";
    
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context);
        
        context.Items[HeaderName] = correlationId;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderName, out var incoming) &&
            !string.IsNullOrWhiteSpace(incoming))
        {
            var trimmed = incoming.ToString().Trim();
            return trimmed.Length <= 128 ? trimmed : trimmed[..128];
        }
        
        return Guid.NewGuid().ToString("N");
    }
}