using PlatformFoundation.Application.Contracts;

namespace PlatformFoundation.Application.Features.Products.ListProducts;

public sealed class ListProductsHandler
{
    private readonly IProductRepository _repo;

    public ListProductsHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<ListProductsResult> Handle(ListProductsQuery query, CancellationToken ct)
    {
        var limit = query.limit;
        var offset = query.offset;

        var total = await _repo.Count(ct);
        var products = await _repo.List(limit, offset, ct);
        
        var items = products
            .Select(p => new ProductListItemResult(p.Id, p.Name, p.Price))
            .ToList();
        
        return new ListProductsResult(limit, offset, total, items);
    }
}
