namespace PlatformFoundation.Application.Features.Products.ListProducts;

public sealed record ListProductsResult(
    int Limit,
    int Offset,
    int Count,
    IReadOnlyList<ProductListItemResult> Items);
