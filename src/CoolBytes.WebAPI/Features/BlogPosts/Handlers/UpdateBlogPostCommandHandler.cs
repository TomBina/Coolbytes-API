using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Builders;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostCommand, BlogPostSummaryViewModel>
    {
        private readonly AppDbContext _context;
        private readonly ExistingBlogPostBuilder _builder;
        private int? _currentImageId;

        public UpdateBlogPostCommandHandler(AppDbContext context, ExistingBlogPostBuilder builder)
        {
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
            var blogPost = await _context.BlogPosts
                                         .Include(b => b.Tags)
                                         .Include(b => b.ExternalLinks)
                                         .Include(b => b.Category)
                                         .SingleOrDefaultAsync(b => b.Id == blogPostId);

            _currentImageId = blogPost.ImageId;

            return blogPost;
        }

        private async Task UpdateBlogPost(BlogPost blogPost, UpdateBlogPostCommand message)
        {
            var tags = message.Tags?.Select(s => new BlogPostTag(s)).ToList() ?? new List<BlogPostTag>();
            var externalLinks = message.ExternalLinks?.Select(el => new ExternalLink(el.Name, el.Url)).ToList() ?? new List<ExternalLink>();
            var category = await _context.Categories.FirstAsync(c => c.Id == message.CategoryId);

            await _builder.UseBlogPost(blogPost)
                          .WithContent(message)
                          .WithImage(message.ImageFile)
                          .WithTags(tags)
                          .WithExternalLinks(externalLinks)
                          .WithCategory(category)
                          .Build();
        }

        private async Task Save(BlogPost blogPost)
        {
            if (blogPost.ImageId != _currentImageId)
                await _context.SaveChangesAsync(() => File.Delete(blogPost.Image.Path));
            else
                await _context.SaveChangesAsync();
        }

        private BlogPostSummaryViewModel ViewModel(BlogPost blogPost)
            => Mapper.Map<BlogPostSummaryViewModel>(blogPost);
    }
}