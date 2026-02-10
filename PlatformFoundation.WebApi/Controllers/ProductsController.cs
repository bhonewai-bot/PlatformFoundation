using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformFoundation.Application.Features.Products.CreateProduct;
using PlatformFoundation.Application.Features.Products.GetProductById;
using PlatformFoundation.WebApi.Contracts.Requests;
using PlatformFoundation.WebApi.Contracts.Responses;
using PlatformFoundation.WebApi.Extensions;

namespace PlatformFoundation.WebApi.Controllers;

[Route("api/products")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductHandler _create;
    private readonly GetProductByIdHandler _getById;

    public ProductsController(CreateProductHandler create, GetProductByIdHandler getById)
    {
        _create = create;
        _getById = getById;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request,
        CancellationToken ct)
    {
        var result = await _create.Handle(new CreateProductCommand(request.Name, request.Price), ct);
        
        var response = new ProductResponse(result.Id, result.Name, result.Price);
        
        return Created($"/api/products/{response.Id}", response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _getById.Handle(new GetProductByIdQuery(id), ct);

        if (result is null)
            return NotFound(new ErrorResponse(
                TraceId: HttpContext.GetCorrelationId(),
                Status: StatusCodes.Status404NotFound,
                Title: "Not found",
                Detail: "Product not found."));
        
        return Ok(new ProductResponse(result.Id, result.Name, result.Price));
    }
}
