using System.ComponentModel.DataAnnotations;

namespace PlatformFoundation.WebApi.Contracts.Products.Requests;

public sealed record UpdateProductRequest(
    [param: Required]
    [param: MaxLength(100)]
    string Name,
    
    [param: Required]
    decimal Price);
