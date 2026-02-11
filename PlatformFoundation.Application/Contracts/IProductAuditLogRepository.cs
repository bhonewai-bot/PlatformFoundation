using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Application.Contracts;

public interface IProductAuditLogRepository
{
    Task Add(ProductAuditLog log, CancellationToken ct);
}
