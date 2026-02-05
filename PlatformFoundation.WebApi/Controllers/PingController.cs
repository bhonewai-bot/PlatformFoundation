using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformFoundation.Application.Features.Ping.GetPing;
using PlatformFoundation.WebApi.Contracts.Responses;

namespace PlatformFoundation.WebApi.Controllers
{
    [Route("api/ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(GetPingHandler handler, CancellationToken ct)
        {
            var result = await handler.Handle(new GetPingQuery(), ct);
            
            return Ok(new PingResponse(result.Message, result.Utc));
        }
    }
}
