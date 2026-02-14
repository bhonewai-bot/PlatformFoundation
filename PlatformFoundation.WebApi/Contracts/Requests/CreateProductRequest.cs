using System.ComponentModel.DataAnnotations;

namespace PlatformFoundation.WebApi.Contracts.Requests;

public sealed record CreateProductRequest(
    [param: Required]
    [param: MinLength(1)]
    [param: MaxLength(100)]
    string Name,
    
    [param: Range(0.01, double.MaxValue)]
    decimal Price
);
