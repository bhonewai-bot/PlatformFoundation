using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformFoundation.Infrastructure.Persistence;

namespace PlatformFoundation.IntegrationTests;

public sealed class ProductsApiTests :  IClassFixture<PostgresFixture>
{
    private readonly PostgresFixture _pg;

    public ProductsApiTests(PostgresFixture pg)
    {
        _pg = pg;
    }

    [Fact]
    public async Task Post_products_returns_201_and_persists()
    {
        using var factory = new TestWebAppFactory(_pg.ConnectionString);
        using var client = factory.CreateClient();

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PlatformFoundationDbContext>();
            await db.Database.MigrateAsync();
        }
        
        client.DefaultRequestHeaders.Add("X-Correlation-ID", "it-test-correlation");

        var req = new
        {
            name = "IntegrationTest Product",
            price = 12.34
        };
        
        var res = await client.PostAsJsonAsync("/api/products", req);

        res.StatusCode.Should().Be(HttpStatusCode.Created);
        res.Headers.Location.Should().NotBeNull();
        
        var get = await client.GetAsync(res.Headers.Location);
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await get.Content.ReadFromJsonAsync<ProductResponse>();
        body.Should().NotBeNull();
        body!.Name.Should().Be("IntegrationTest Product");
        body.Price.Should().Be(12.34m);
        
        res.Headers.TryGetValues("X-Correlation-ID", out var cids).Should().BeTrue();
    }
    
    private sealed record ProductResponse(Guid Id, string Name, decimal Price);
}
