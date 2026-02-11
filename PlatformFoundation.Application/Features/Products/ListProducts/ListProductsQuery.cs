namespace PlatformFoundation.Application.Features.Products.ListProducts;

public sealed record ListProductsQuery(int limit = 20, int offset = 0);
