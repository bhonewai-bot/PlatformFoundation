using Microsoft.AspNetCore.Mvc;
using PlatformFoundation.Application;
using PlatformFoundation.Application.Contracts;
using PlatformFoundation.WebApi.Contracts.Responses;
using PlatformFoundation.WebApi.Extensions;
using PlatformFoundation.WebApi.Infrastructure;
using PlatformFoundation.WebApi.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

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

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    options.EnrichDiagnosticContext = (diagnostic, context) =>
    {
        if (context.Items.TryGetValue("X-Correlation-ID", out var cidObj) && cidObj is string cid)
            diagnostic.Set("CorrelationId", cid);
        
        diagnostic.Set("TraceIdentifier", context.TraceIdentifier);
        diagnostic.Set("Endpoint", context.GetEndpoint()?.DisplayName);
        diagnostic.Set("ClientIp", context.Connection.RemoteIpAddress?.ToString());
        diagnostic.Set("UserAgent", context.Request.Headers.UserAgent.ToString());
    };
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();