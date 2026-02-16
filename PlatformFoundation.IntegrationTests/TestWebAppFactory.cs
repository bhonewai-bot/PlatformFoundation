using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace PlatformFoundation.IntegrationTests;

public sealed class TestWebAppFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public TestWebAppFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var overrides = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = _connectionString,
                ["ASPNETCORE_ENVIRONMENT"] = "Testing"
            };
            
            config.AddInMemoryCollection(overrides);
        });
    }
}
