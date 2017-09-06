using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Get(GetBlogPostsQuery query) => Ok(await _mediator.Send(query));
    }
}
