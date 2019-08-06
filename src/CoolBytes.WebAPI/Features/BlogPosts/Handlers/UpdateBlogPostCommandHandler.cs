using CoolBytes.Core.Builders;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostCommand, BlogPostSummaryViewModel>
    {
        private readonly AppDbContext _dbContext;
        private readonly HandlerContext<BlogPostSummaryViewModel> _context;
        private readonly ExistingBlogPostBuilder _builder;
        private int? _currentImageId;

        public UpdateBlogPostCommandHandler(HandlerContext<BlogPostSummaryViewModel> context, ExistingBlogPostBuilder builder)
        {
            _dbContext = context.DbContext;
            _context = context;
            _builder = builder;
        }

        public async Task<BlogPostSummaryViewModel> Handle(UpdateBlogPostCommand message, CancellationToken cancellationToken)
        {
            var blogPost = await GetBlogPost(message.Id);

            await UpdateBlogPost(blogPost, message);
            await Save(blogPost);

            return ViewModel(blogPost);
        }

        private async Task<BlogPost> GetBlogPost(int blogPostId)
        {
            var blogPost = await _dbContext.BlogPosts
                                         .Include(b => b.Tags)
                                         .Include(b => b.ExternalLinks)
                                         .Include(b => b.Category)
                                         .Include(b => b.MetaTags)
                                         .SingleOrDefaultAsync(b => b.Id == blogPostId);

            _currentImageId = blogPost.ImageId;

            return blogPost;
        }

        private async Task UpdateBlogPost(BlogPost blogPost, UpdateBlogPostCommand message)
        {
            var tags = message.Tags?.Select(s => new BlogPostTag(s)).ToList() ?? new List<BlogPostTag>();
            var externalLinks = message.ExternalLinks?.Select(el => new ExternalLink(el.Name, el.Url)).ToList() ?? new List<ExternalLink>();
            var category = await _dbContext.Categories.FirstAsync(c => c.Id == message.CategoryId);
            var metaTags = message.MetaTags?.Select(m => new MetaTag(m.Name, m.Value));

            await _builder.UseBlogPost(blogPost)
                          .WithContent(message)
                          .WithImage(message.ImageFile)
                          .WithTags(tags)
                          .WithExternalLinks(externalLinks)
                          .WithCategory(category)
                          .WithMetaTags(metaTags)
                          .Build();
        }

        private async Task Save(BlogPost blogPost)
        {
            if (blogPost.ImageId != _currentImageId)
                await _dbContext.SaveChangesAsync(() => File.Delete(blogPost.Image.Path));
            else
                await _dbContext.SaveChangesAsync();
        }

        private BlogPostSummaryViewModel ViewModel(BlogPost blogPost)
            => _context.Map(blogPost);
    }
}