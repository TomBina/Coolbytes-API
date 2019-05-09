using CoolBytes.Core.Builders;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class AddBlogPostCommandHandler : IRequestHandler<AddBlogPostCommand, BlogPostSummaryViewModel>
    {
        private readonly AppDbContext _dbContext;
        private readonly HandlerContext<BlogPostSummaryViewModel> _context;
        private readonly BlogPostBuilder _builder;

        public AddBlogPostCommandHandler(HandlerContext<BlogPostSummaryViewModel> context, BlogPostBuilder builder) 
        {
            _dbContext = context.DbContext;
            _context = context;
            _builder = builder;
        }

        public async Task<BlogPostSummaryViewModel> Handle(AddBlogPostCommand message, CancellationToken cancellationToken)
        {
            var blogPost = await CreateBlogPost(message);

            await Save(blogPost);

            return ViewModel(blogPost);
        }

        private async Task<BlogPost> CreateBlogPost(AddBlogPostCommand message)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == message.CategoryId);
            var tags = message.Tags?.Select(s => new BlogPostTag(s)).ToList();
            var externalLinks = message.ExternalLinks?.Select(el => new ExternalLink(el.Name, el.Url)).ToList();

            return await _builder.WrittenByCurrentAuthor()
                                 .WithContent(message)
                                 .WithImage(message.ImageFile)
                                 .WithTags(tags)
                                 .WithExternalLinks(externalLinks)
                                 .WithCategory(category)
                                 .Build();
}

        private async Task Save(BlogPost blogPost)
        {
            _dbContext.BlogPosts.Add(blogPost);

            if (blogPost.Image != null)
                await _dbContext.SaveChangesAsync(() => File.Delete(blogPost.Image.Path));
            else
                await _dbContext.SaveChangesAsync();
        }

        private BlogPostSummaryViewModel ViewModel(BlogPost blogPost) 
                => _context.Map(blogPost);
    }
}