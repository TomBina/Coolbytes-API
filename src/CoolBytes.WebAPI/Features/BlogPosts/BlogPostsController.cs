using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using CoolBytes.WebAPI.Features.BlogPosts.Validators;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostSummaryViewModel>>> Get([FromQuery]GetBlogPostsQuery query)
            => this.OkOrNotFound(await _mediator.Send(query));

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostViewModel>> Get(int id)
        {
            var query = new GetBlogPostQuery() { Id = id };
            var blogPost = await _mediator.Send(query);

            if (!blogPost)
            {
                return NotFound();
            }

            return blogPost.Payload;
        }

        [Authorize("admin")]
        [HttpPost]
        public async Task<ActionResult<BlogPostSummaryViewModel>> Add([FromForm] AddBlogPostCommand command, [FromForm] string externalLinks)
        {
            var links = ValidateExternalLinks(externalLinks);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.ExternalLinks = links;

            return await _mediator.Send(command);
        }

        private IEnumerable<ExternalLinkDto> ValidateExternalLinks(string externalLinks)
        {
            if (externalLinks == null || externalLinks == "[]")
                return null;

            var settings = new JsonSerializerSettings();
            settings.Error = (sender, args) => ModelState.AddModelError(nameof(externalLinks), "Invalid json");
            var links = JsonConvert.DeserializeObject<List<ExternalLinkDto>>(externalLinks, settings);

            var validator = new ExternalLinkDtoValidator();
            var errors = links.Select(l => validator.Validate(l)).Where(r => !r.IsValid);

            foreach (var error in errors)
                error.AddToModelState(ModelState, nameof(externalLinks));

            return links;
        }

        [Authorize("admin")]
        [HttpGet("update/{id}")]
        public async Task<ActionResult<BlogPostUpdateViewModel>> Update(int id)
        {
            var command = new UpdateBlogPostQuery() { Id = id };

            return this.OkOrNotFound(await _mediator.Send(command));
        }

        [Authorize("admin")]
        [HttpPut("update")]
        public async Task<ActionResult<BlogPostSummaryViewModel>> Update([FromForm] UpdateBlogPostCommand command, [FromForm] string externalLinks)
        {
            var links = ValidateExternalLinks(externalLinks);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.ExternalLinks = links;

            return await _mediator.Send(command);
        }

        [Authorize("admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteBlogPostCommand() { Id = id };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}