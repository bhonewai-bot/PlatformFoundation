using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PlatformFoundation.Application.Features.Products.CreateProduct;
using PlatformFoundation.Application.Features.Products.GetProductById;
using PlatformFoundation.Application.Features.Products.ListProducts;
using PlatformFoundation.WebApi.Contracts.Products.Requests;
using PlatformFoundation.WebApi.Contracts.Products.Responses;
using PlatformFoundation.WebApi.Contracts.Realtime.Events;
using PlatformFoundation.WebApi.Errors;
using PlatformFoundation.WebApi.Extensions;
using PlatformFoundation.WebApi.Realtime;

namespace PlatformFoundation.WebApi.Controllers;

[Route("api/products")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductHandler _create;
    private readonly GetProductByIdHandler _getById;
    private readonly ListProductsHandler _list;
    private readonly IRealtimePublisher _realtime;

    public ProductsController(CreateProductHandler create, GetProductByIdHandler getById, ListProductsHandler list, IRealtimePublisher realtime)
    {
        _create = create;
        _getById = getById;
        _list = list;
        _realtime = realtime;
    }

    [EnableRateLimiting("write-strict")]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request,
        CancellationToken ct)
    {
        var result = await _create.Handle(new CreateProductCommand(request.Name, request.Price), ct);
        
        var response = new ProductResponse(result.Id, result.Name, result.Price);
        
        await _realtime.PublishToTopic(
            topic: "products",
            type: "product.created",
            version: 1,
            data: new ProductCreatedEvent(response.Id, response.Name, response.Price),
            ct: ct);
        
        return Created($"/api/products/{response.Id}", response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _getById.Handle(new GetProductByIdQuery(id), ct);

        if (result is null)
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
