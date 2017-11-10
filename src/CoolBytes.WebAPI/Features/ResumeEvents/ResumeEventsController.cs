using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.ResumeEvents
{
    [Authorize("admin")]
    [Route("api/[controller]")]
    public class ResumeEventsController : Controller
    {
        private readonly IMediator _mediator;

        public ResumeEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{authorid}")]
        public async Task<IActionResult> Get(GetResumeEventsQuery message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return this.OkOrNotFound(await _mediator.Send(message));
        }

        [HttpGet("event/{id}")]
        public async Task<IActionResult> Get(GetResumeEventQuery message)
            => this.OkOrNotFound(await _mediator.Send(message));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddResumeEventCommand message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(message));
        }
    
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateResumeEventCommand message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(message));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(DeleteResumeEventCommand message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Send(message);

            return NoContent();
        }
    }
}
