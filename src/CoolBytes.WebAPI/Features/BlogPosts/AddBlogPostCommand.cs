using CoolBytes.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CoolBytes.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class AddBlogPostCommand : IRequest<BlogPostViewModel>
    {
        public string Subject { get; set; }
        public string ContentIntro { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IFormFile File { get; set; }
    }
}