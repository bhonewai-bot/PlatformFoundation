using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformFoundation.WebApi.Contracts.Common;
using PlatformFoundation.WebApi.Contracts.Products.Responses;

namespace PlatformFoundation.WebApi.Controllers;

[Route("api/info")]
[ApiController]
public class InfoController : ControllerBase
{
    [HttpGet]
    public ActionResult<AppInfoResponse> Get()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
        
        var version = Assembly.GetExecutingAssembly()
            .GetName()
            .Version?
            .ToString() ?? "Unknown";
        
        var response = new AppInfoResponse(
            AppName: "PlatformFoundation",
            Environment: env,
            Version: version,
            UtcNow: DateTime.UtcNow);
        
        return Ok(response);
    }
}
