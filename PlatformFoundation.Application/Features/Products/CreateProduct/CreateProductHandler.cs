using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Application.Features.Products.CreateProduct;

public sealed class CreateProductHandler
{
    private readonly IProductRepository _repo;

    public CreateProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        var product = Product.Create(cmd.Name, cmd.Price);
        
        await _repo.Add(product, ct);
        
        return new CreateProductResult(product.Id, product.Name, product.Price);
    }
}