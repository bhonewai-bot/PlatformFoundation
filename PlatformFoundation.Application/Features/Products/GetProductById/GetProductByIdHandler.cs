using PlatformFoundation.Application.Contracts;

namespace PlatformFoundation.Application.Features.Products.GetProductById;

public sealed class GetProductByIdHandler
{
    private readonly IProductRepository _repo;

    public GetProductByIdHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<GetProductByIdResult?> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await _repo.GetById(query.Id, ct);
        if (product is null) return null;

        return new GetProductByIdResult(product.Id, product.Name, product.Price);
    }
}
