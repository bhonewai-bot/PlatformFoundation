using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PlatformFoundation.Application;
using PlatformFoundation.Application.Contracts;
using PlatformFoundation.Infrastructure;
using PlatformFoundation.WebApi.Contracts.Responses;
using PlatformFoundation.WebApi.Extensions;
using PlatformFoundation.WebApi.Middlewares;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live", "ready" });
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

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

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    options.GetLevel = (context, elapsed, ex) =>
    {
        var path = context.Request.Path.Value ?? "";
        if (path.StartsWith("/health")) return LogEventLevel.Verbose;
        
        if (ex != null) return LogEventLevel.Error;
        if (context.Response.StatusCode >= 500) return LogEventLevel.Error;

        return LogEventLevel.Information;
    };

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/live", new HealthCheckOptions()
{
    Predicate = r => r.Tags.Contains("live"),
    ResponseWriter = HealthResponseWriter.WriteJsonResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions()
{
    Predicate = r => r.Tags.Contains("ready"),
    ResponseWriter = HealthResponseWriter.WriteJsonResponse
});

app.Run();
