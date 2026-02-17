using System.ComponentModel.DataAnnotations;

namespace PlatformFoundation.WebApi.Contracts.Products.Requests;

public sealed record CreateProductRequest(
    [param: Required]
    [param: MaxLength(100)]
    string Name,
    
    [param: Range(0.01, double.MaxValue)]
    decimal Price
);
