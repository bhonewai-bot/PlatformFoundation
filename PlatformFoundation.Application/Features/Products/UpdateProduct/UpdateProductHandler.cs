using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;
using PlatformFoundation.Domain.Exceptions;

namespace PlatformFoundation.Application.Features.Products.UpdateProduct;

public class UpdateProductHandler
{
    private readonly IProductRepository _products;
    private readonly IUnitOfWork _uow;

    public UpdateProductHandler(IProductRepository products, IUnitOfWork uow)
    {
        _products = products;
        _uow = uow;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand cmd, CancellationToken ct)
    {
        var exists = await _products.GetById(cmd.Id, ct);
        if (exists is null) return null;

        var isChangingName = !string.Equals(
            exists.Name.Trim(),
            cmd.Name.Trim(),
            StringComparison.OrdinalIgnoreCase);
        
        if (isChangingName && await _products.ExistsByNameExceptId(cmd.Name, cmd.Id, ct))
            throw new DomainConflictException("Product name already exists");

        var updated = await _products.Update(cmd.Id, cmd.Name, cmd.Price, ct);
        if (updated is null) return null;

        await _uow.SaveChangesAsync(ct);
        
        return new UpdateProductResult(updated.Id, updated.Name, updated.Price);
    } 
}
