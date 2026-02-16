using Testcontainers.PostgreSql;

namespace PlatformFoundation.IntegrationTests;

public sealed class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder()
        .WithDatabase("platformfoundation_test")
        .WithUsername("pf_test")
        .WithPassword("pf_test")
        .Build();
    
    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}
