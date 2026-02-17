using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformFoundation.Application.Features.Ping.GetPing;
using PlatformFoundation.WebApi.Contracts.Ping.Responses;

namespace PlatformFoundation.WebApi.Controllers
{
    [Route("api/ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly GetPingHandler _handler;

        public PingController(GetPingHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<ActionResult<PingResponse>> Get(GetPingHandler handler, CancellationToken ct)
        {
            var result = await handler.Handle(new GetPingQuery(), ct);
            
            return Ok(new PingResponse(result.Message, result.Utc));
        }
    }
}
