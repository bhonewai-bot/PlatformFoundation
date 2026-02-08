using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PlatformFoundation.WebApi.Extensions;

public static class HealthResponseWriter
{
    public static Task WriteJsonResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
            })
        };
        
        return context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(payload));
    }
}
