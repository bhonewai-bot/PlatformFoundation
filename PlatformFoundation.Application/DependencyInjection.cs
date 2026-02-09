using Microsoft.Extensions.DependencyInjection;
using PlatformFoundation.Application.Features.Ping.GetPing;
using PlatformFoundation.Application.Features.Products.CreateProduct;

namespace PlatformFoundation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetPingHandler>();
        services.AddScoped<CreateProductHandler>();
        
        return services;
    }
}
