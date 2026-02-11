using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;
using PlatformFoundation.Infrastructure.Persistence;

namespace PlatformFoundation.Infrastructure.Repositories;

public sealed class EfProductAuditLogRepository : IProductAuditLogRepository
{
    private readonly PlatformFoundationDbContext _db;

    public EfProductAuditLogRepository(PlatformFoundationDbContext db)
    {
        _db = db;
    }

    public async Task Add(ProductAuditLog log, CancellationToken ct)
    {
        await _db.ProductAuditLogs.AddAsync(log, ct);
    }
}
