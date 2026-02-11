using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Application.Contracts;

public interface IProductRepository
{
    Task Add(Product product, CancellationToken ct);
    Task<Product?> GetById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Product>> List(int limit, int offset, CancellationToken ct);
    Task<int> Count(CancellationToken ct);
}
