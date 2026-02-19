using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Application.Features.Products.GetProductById;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Application.Features.Products.DeleteProduct;

public class DeleteProductHandler
{
    private readonly IProductRepository _products;
    private readonly IProductAuditLogRepository _audit;
    private readonly IUnitOfWork _uow;

    public DeleteProductHandler(IProductRepository products, IProductAuditLogRepository audit, IUnitOfWork uow)
    {
        _products = products;
        _audit = audit;
        _uow = uow;
    }

    public async Task<DeleteProductResult?> Handle(DeleteProductCommand cmd, CancellationToken ct)
    {
        var ok = await _products.Delete(cmd.Id, ct);
        if (!ok) return null;

        await _audit.Add(ProductAuditLog.Deleted(cmd.Id), ct);
        await _uow.SaveChangesAsync(ct);
        
        return new DeleteProductResult(cmd.Id);
    }
}
