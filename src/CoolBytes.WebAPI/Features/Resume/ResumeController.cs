using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Features.Resume.CQ;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.Resume
{
    [Route("api/[controller]")]
    public class ResumeController : Controller
    {
        private readonly IMediator _mediator;

        public ResumeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Get(GetResumeQuery message) 
            => this.OkOrNotFound(await _mediator.Send(message));

        public async Task<IActionResult> Add(AddResumeEventCommand message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(message));
        }

        public async Task<IActionResult> Update(UpdateResumeEventCommand message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(message));
        }
    }
}
