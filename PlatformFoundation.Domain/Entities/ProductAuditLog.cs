namespace PlatformFoundation.Domain.Entities;

public class ProductAuditLog
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string Action { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    
    private ProductAuditLog() { }

    private ProductAuditLog(Guid productId, string action)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Action = action;
        CreatedAt = DateTime.UtcNow;
    }

    public static ProductAuditLog Created(Guid productId) => new ProductAuditLog(productId, "created");
    
    public static ProductAuditLog Deleted(Guid productId) => new ProductAuditLog(productId, "deleted");
}
