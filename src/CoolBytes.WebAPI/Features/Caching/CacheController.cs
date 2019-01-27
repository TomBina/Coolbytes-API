using System.Threading.Tasks;
using CoolBytes.WebAPI.Features.Categories.CQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Caching
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : Controller
    {
        private readonly IMediator _mediator;

        public CacheController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize("admin")]
        [HttpDelete]
        [ProducesResponseType(203)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Invalidate()
        {
            var message = new InvalidateCacheCommand();
            var result = await _mediator.Send(message);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}