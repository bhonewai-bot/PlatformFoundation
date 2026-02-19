using Microsoft.Extensions.DependencyInjection;
using PlatformFoundation.Application.Features.Ping.GetPing;
using PlatformFoundation.Application.Features.Products.CreateProduct;
using PlatformFoundation.Application.Features.Products.DeleteProduct;
using PlatformFoundation.Application.Features.Products.GetProductById;
using PlatformFoundation.Application.Features.Products.ListProducts;
using PlatformFoundation.Application.Features.Products.UpdateProduct;

namespace PlatformFoundation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetPingHandler>();
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<ListProductsHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        
        return services;
    }
}
