using System.Collections;
using CoolBytes.WebAPI.Extensions;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.BlogPosts.Validators;
using FluentValidation.AspNetCore;
using FluentValidation.Results;

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
        public async Task<IActionResult> Add([FromForm] AddBlogPostCommand command, [FromForm] string externalLinks)
        {
            var links = ValidateExternalLinks(externalLinks);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.ExternalLinks = links;

            return Ok(await _mediator.Send(command));
        }

        private IEnumerable<ExternalLinkDto> ValidateExternalLinks(string externalLinks)
        {
            var settings = new JsonSerializerSettings();
            settings.Error = (sender, args) => ModelState.AddModelError(nameof(externalLinks), "Invalid json");
            var links = JsonConvert.DeserializeObject<List<ExternalLinkDto>>(externalLinks, settings);

            if (links == null)
                return null;

            var validator = new ExternalLinkDtoValidator();
            var errors = links.Select(l => validator.Validate(l)).Where(r => !r.IsValid);

            foreach (var error in errors)
                error.AddToModelState(ModelState, nameof(externalLinks));

            return links;
        }

        [Authorize("Admin")]
        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(UpdateBlogPostQuery query) => this.OkOrNotFound(await _mediator.Send(query));

        [Authorize("admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateBlogPostCommand command, [FromForm] string externalLinks)
        {
            var links = ValidateExternalLinks(externalLinks);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.ExternalLinks = links;

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