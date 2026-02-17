using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PlatformFoundation.Application.Features.Products.CreateProduct;
using PlatformFoundation.Application.Features.Products.GetProductById;
using PlatformFoundation.Application.Features.Products.ListProducts;
using PlatformFoundation.WebApi.Contracts.Products.Requests;
using PlatformFoundation.WebApi.Contracts.Products.Responses;
using PlatformFoundation.WebApi.Errors;
using PlatformFoundation.WebApi.Extensions;

namespace PlatformFoundation.WebApi.Controllers;

[Route("api/products")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductHandler _create;
    private readonly GetProductByIdHandler _getById;
    private readonly ListProductsHandler _list;

    public ProductsController(CreateProductHandler create, GetProductByIdHandler getById, ListProductsHandler list)
    {
        _create = create;
        _getById = getById;
        _list = list;
    }

    [EnableRateLimiting("write-strict")]
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
            /*return NotFound(new ErrorResponse(
                TraceId: HttpContext.GetCorrelationId(),
                Status: StatusCodes.Status404NotFound,
                Title: "Not found",
                Detail: "Products not found."));*/
            return NotFound(ErrorFactory.NotFound(HttpContext.GetCorrelationId(), "Products not found."));
        
        return Ok(new ProductResponse(result.Id, result.Name, result.Price));
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> List([FromQuery] int limit = 20, [FromQuery] int offset = 0, CancellationToken ct = default)
    {
        var result = await _list.Handle(new ListProductsQuery(limit, offset), ct);
        
        var response = new ListProductsResponse(
            result.Limit,
            result.Offset,
            result.Count,
            result.Items.Select(x => new ProductListItemResponse(x.Id, x.Name, x.Price)).ToList());
        
        return Ok(response);
    }
}
