namespace PlatformFoundation.WebApi.Contracts.Products.Responses;

public record ListProductsResponse(
    int Limit,
    int Offset,
    int Total,
    IReadOnlyList<ProductListItemResponse> Items);
