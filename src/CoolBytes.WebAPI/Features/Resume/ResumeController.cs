using CoolBytes.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Resume
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResumeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<ResumeViewModel>> Get(int authorId)
        {
            var query = new GetResumeQuery() { AuthorId = authorId };

            return this.OkOrNotFound(await _mediator.Send(query));
        }
    }
}
