namespace PlatformFoundation.WebApi.Contracts.Products.Responses;

public sealed record ProductListItemResponse(Guid Id, string Name, decimal Price);
