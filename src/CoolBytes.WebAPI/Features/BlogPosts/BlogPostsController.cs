using CoolBytes.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize("admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddBlogPostCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }

        [Authorize("Admin")]
        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(UpdateBlogPostQuery query) => this.OkOrNotFound(await _mediator.Send(query));

        [Authorize("admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateBlogPostCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }

        [Authorize("admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(DeleteBlogPostCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}