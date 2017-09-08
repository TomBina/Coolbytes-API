using CoolBytes.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class AddBlogPostCommand : IRequest<BlogPostViewModel>
    {
        public string Subject { get; set; }
        public string ContentIntro { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public IEnumerable<BlogPostTag> BlogPostTags { get; set; }
    }

    public class AddBlogPostCommandHandler : IAsyncRequestHandler<AddBlogPostCommand, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;

        public AddBlogPostCommandHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BlogPostViewModel> Handle(AddBlogPostCommand message)
        {
            var author = _appDbContext.Authors.Find(message.AuthorId);
            var blogPost = new BlogPost(message.Subject, message.ContentIntro, message.Content, author);

            if (message.BlogPostTags != null)
            {
                blogPost.AddTags(message.BlogPostTags);
            }

            await _appDbContext.SaveChangesAsync();

            return Mapper.Map<BlogPostViewModel>(blogPost);
        }
    }
}