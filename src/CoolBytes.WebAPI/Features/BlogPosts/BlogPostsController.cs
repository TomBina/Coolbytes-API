using CoolBytes.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    [Route("api/[controller]")]
    public class BlogPostsController : Controller
    {
        private readonly IMediator _mediator;

        public BlogPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(GetBlogPostsQuery query) => this.OkOrNotFound(await _mediator.Send(query));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(GetBlogPostQuery query) => this.OkOrNotFound(await _mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> Post(AddBlogPostCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateBlogPostCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteBlogPostCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}