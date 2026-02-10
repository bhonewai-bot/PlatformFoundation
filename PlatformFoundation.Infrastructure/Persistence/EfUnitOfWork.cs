using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Infrastructure.AppDbContext;

namespace PlatformFoundation.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly PlatformFoundationDbContext _db;

    public EfUnitOfWork(PlatformFoundationDbContext db)
    {
        _db = db;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}
