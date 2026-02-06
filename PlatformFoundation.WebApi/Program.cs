using Microsoft.AspNetCore.Mvc;
using PlatformFoundation.Application;
using PlatformFoundation.Application.Contracts;
using PlatformFoundation.WebApi.Contracts.Responses;
using PlatformFoundation.WebApi.Extensions;
using PlatformFoundation.WebApi.Infrastructure;
using PlatformFoundation.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddScoped<IProductRepository, InMemoryProductRepository>();

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var traceId = context.HttpContext.GetCorrelationId();

        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

        var payload = new ErrorResponse(
            TraceId: traceId,
            Status: StatusCodes.Status400BadRequest,
            Title: "Validation failed",
            Detail: "One or more validation errors occured.",
            Errors: errors);
        
        return new BadRequestObjectResult(payload);
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();