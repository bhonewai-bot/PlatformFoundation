using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformFoundation.Infrastructure.AppDbContext;

namespace PlatformFoundation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configure)
    {
        var connectionString = configure.GetConnectionString("Default");
        
        services.AddDbContext<PlatformFoundationDbContext>(options => 
            options.UseNpgsql(connectionString));
        
        return services;
    }
}
