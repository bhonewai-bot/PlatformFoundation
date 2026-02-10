using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Application.Features.Products.CreateProduct;

public sealed class CreateProductHandler
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateProductHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        var product = Product.Create(cmd.Name, cmd.Price);
        
        await _repo.Add(product, ct);
        await _uow.SaveChangesAsync(ct);
        
        return new CreateProductResult(product.Id, product.Name, product.Price);
    }
}
