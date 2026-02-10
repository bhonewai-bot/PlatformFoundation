using Microsoft.EntityFrameworkCore;
using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;
using PlatformFoundation.Infrastructure.AppDbContext;

namespace PlatformFoundation.Infrastructure.Repositories;

public sealed class EfProductRepository : IProductRepository
{
    private readonly PlatformFoundationDbContext _db;

    public EfProductRepository(PlatformFoundationDbContext db)
    {
        _db = db;
    }

    public async Task Add(Product product, CancellationToken ct)
    {
        await _db.Products.AddAsync(product, ct);
    }

    public Task<Product?> GetById(Guid id, CancellationToken ct)
    {
        return _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
