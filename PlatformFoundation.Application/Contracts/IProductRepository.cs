using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Application.Contracts;

public interface IProductRepository
{
    Task Add(Product product, CancellationToken ct);
    Task<Product?> GetById(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Product>> List(int limit, int offset, CancellationToken ct);
    Task<int> Count(CancellationToken ct);
    Task<bool> ExistsByName(string name, CancellationToken ct);
    Task<bool> ExistsByNameExceptId(string name, Guid excludeId, CancellationToken ct);
    Task<Product?> Update(Guid id, string name, decimal price, CancellationToken ct);
    Task<bool> Delete(Guid id, CancellationToken ct);
}
