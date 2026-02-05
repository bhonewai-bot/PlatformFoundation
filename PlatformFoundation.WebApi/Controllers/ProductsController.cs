using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformFoundation.Application.Features.Products.CreateProduct;
using PlatformFoundation.WebApi.Contracts.Requests;
using PlatformFoundation.WebApi.Contracts.Responses;

namespace PlatformFoundation.WebApi.Controllers;

[Route("api/products")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductHandler _create;

    public ProductsController(CreateProductHandler create)
    {
        _create = create;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request,
        CancellationToken ct)
    {
        var result = await _create.Handle(new CreateProductCommand(request.Name, request.Price), ct);
        
        var response = new ProductResponse(result.Id, result.Name, result.Price);
        
        return Created($"/api/products/{response.Id}", response);
    }
}