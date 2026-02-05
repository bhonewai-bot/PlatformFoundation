using Microsoft.Extensions.DependencyInjection;
using PlatformFoundation.Application.Features.Ping.GetPing;

namespace PlatformFoundation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetPingHandler>();
        
        return services;
    }
}