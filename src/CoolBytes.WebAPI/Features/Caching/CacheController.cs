using System.Threading.Tasks;
using CoolBytes.WebAPI.Features.Categories.CQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Caching
{
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
        public async Task<ActionResult> Truncate()
        {
            var message = new TruncateCacheCommand();
            var result = await _mediator.Send(message);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}