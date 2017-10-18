using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Features.Resume.CQ;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Resume
{
    [Route("api/[controller]")]
    public class ResumeEventsController : Controller
    {
        private readonly IMediator _mediator;

        public ResumeEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> GetResumeEvents(GetResumeQuery message) 
            => this.OkOrNotFound(await _mediator.Send(message));

        public async Task<IActionResult> AddResumeEvent(AddResumeEventCommand message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(message));
        }

        public async Task<IActionResult> UpdateResumeEvent(UpdateResumeEventCommand message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(message));
        }
    }
}
