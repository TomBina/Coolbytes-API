using System.Collections.Generic;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Features.ResumeEvents
{
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<ResumeEventViewModel>>> GetByAuthorId(int authorId)
        {
            var query = new GetResumeEventsQuery() { AuthorId = authorId };

            return this.OkOrNotFound(await _mediator.Send(query));
        }

        [HttpGet("event/{id}")]
        public async Task<ActionResult<ResumeEventViewModel>> GetById(int id)
        {
            var query = new GetResumeEventQuery() { Id = id };
            return this.OkOrNotFound(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult<ResumeEventViewModel>> Add(AddResumeEventCommand message)
            => await _mediator.Send(message);

        [HttpPut]
        public async Task<ResumeEventViewModel> Update(UpdateResumeEventCommand message)
            => await _mediator.Send(message);

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteResumeEventCommand() { Id = id };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
