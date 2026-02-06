using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.WebApi.Infrastructure.DevOnly;

public sealed class DevOnlyInMemoryProductRepository : IProductRepository
{
    private static readonly Dictionary<Guid, Product> Store = new();

    public Task Add(Product product, CancellationToken ct)
    {
        Store[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task<Product?> GetById(Guid id, CancellationToken ct)
    {
        Store.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }
}