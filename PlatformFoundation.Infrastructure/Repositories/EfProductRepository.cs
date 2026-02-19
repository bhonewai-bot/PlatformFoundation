using Microsoft.EntityFrameworkCore;
using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;
using PlatformFoundation.Infrastructure.Persistence;

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

    public async Task<IReadOnlyList<Product>> List(int limit, int offset, CancellationToken ct)
    {
        if (limit <= 0) limit = 20;
        if (limit > 100) limit = 100;
        if (offset < 0) offset = 0;
        
        return await _db.Products
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);
    }

    public Task<int> Count(CancellationToken ct)
    {
        return _db.Products.CountAsync(ct);
    }

    public Task<bool> ExistsByName(string name, CancellationToken ct)
    {
        var normalized = name.Trim().ToLower();
        return _db.Products
            .AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == normalized, ct);
    }

    public Task<bool> ExistsByNameExceptId(string name, Guid excludeId, CancellationToken ct)
    {
        var normalized = name.Trim().ToLower();
        return _db.Products
            .AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == normalized && x.Id != excludeId, ct);
    }

    public async Task<Product?> Update(Guid id, string name, decimal price, CancellationToken ct)
    {
        var existingProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (existingProduct is null)
            return null;
        
        existingProduct.Update(name, price);
        
        return existingProduct;
    }

    public async Task<bool> Delete(Guid id, CancellationToken ct)
    {
        var existingProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (existingProduct is null)
            return false;
        
        _db.Products.Remove(existingProduct);
        return true;
    }
}
