namespace PlatformFoundation.Application.Features.Products.GetProductById;

public sealed record GetProductByIdResult(Guid Id, string Name, decimal Price);
