namespace PlatformFoundation.WebApi.Contracts.Responses;

public record ListProductsResponse(
    int Limit,
    int Offset,
    int Total,
    IReadOnlyList<ProductListItemResponse> Items);
