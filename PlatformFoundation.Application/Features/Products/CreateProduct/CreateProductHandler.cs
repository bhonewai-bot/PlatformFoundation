using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Application.Features.Products.CreateProduct;

public sealed class CreateProductHandler
{
    private readonly IProductRepository _products;
    private readonly IProductAuditLogRepository _audit;
    private readonly IUnitOfWork _uow;

    public CreateProductHandler(IProductRepository repo, IProductAuditLogRepository audit, IUnitOfWork uow)
    {
        _products = repo;
        _audit = audit;
        _uow = uow;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        var product = Product.Create(cmd.Name, cmd.Price);
        
        await _products.Add(product, ct);

        var log = ProductAuditLog.Created(product.Id);
        await _audit.Add(log, ct);
        
        await _uow.SaveChangesAsync(ct);
        
        return new CreateProductResult(product.Id, product.Name, product.Price);
    }
}
