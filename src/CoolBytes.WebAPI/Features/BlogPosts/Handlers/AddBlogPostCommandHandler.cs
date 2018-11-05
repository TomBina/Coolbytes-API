using AutoMapper;
using CoolBytes.Core.Builders;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class AddBlogPostCommandHandler : IRequestHandler<AddBlogPostCommand, BlogPostSummaryViewModel>
    {
        private readonly AppDbContext _context;
        private readonly BlogPostBuilder _builder;

        public AddBlogPostCommandHandler(AppDbContext context, BlogPostBuilder builder) 
        {
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
            var tags = message.Tags?.Select(s => new BlogPostTag(s)).ToList();
            var externalLinks = message.ExternalLinks?.Select(el => new ExternalLink(el.Name, el.Url)).ToList();

            return await _builder.WrittenByCurrentAuthor()
                                        .WithContent(message)
                                        .WithImage(message.ImageFile)
                                        .WithTags(tags)
                                        .WithExternalLinks(externalLinks)
                                        .Build();
        }

        private async Task Save(BlogPost blogPost)
        {
            _context.BlogPosts.Add(blogPost);

            if (blogPost.Image != null)
                await _context.SaveChangesAsync(() => File.Delete(blogPost.Image.Path));
            else
                await _context.SaveChangesAsync();
        }

        private BlogPostSummaryViewModel ViewModel(BlogPost blogPost) 
                => Mapper.Map<BlogPostSummaryViewModel>(blogPost);
    }
}