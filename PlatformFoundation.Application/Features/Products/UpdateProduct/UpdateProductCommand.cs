namespace PlatformFoundation.Application.Features.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, decimal Price);
