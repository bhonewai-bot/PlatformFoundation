using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Infrastructure.AppDbContext;
using PlatformFoundation.Infrastructure.Persistence;
using PlatformFoundation.Infrastructure.Repositories;

namespace PlatformFoundation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configure)
    {
        var connectionString = configure.GetConnectionString("Default");
        
        services.AddDbContext<PlatformFoundationDbContext>(options => 
            options.UseNpgsql(connectionString));

        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IProductAuditLogRepository, EfProductAuditLogRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        
        return services;
    }
}
