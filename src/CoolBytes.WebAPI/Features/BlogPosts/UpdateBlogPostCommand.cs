using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoolBytes.Core.Models;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class UpdateBlogPostCommand : IRequest<BlogPostViewModel>
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string ContentIntro { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public IEnumerable<BlogPostTag> BlogPostTags { get; set; }
    }
}
