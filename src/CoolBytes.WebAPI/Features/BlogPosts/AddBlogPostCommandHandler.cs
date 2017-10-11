using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Builders;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class AddBlogPostCommandHandler : IAsyncRequestHandler<AddBlogPostCommand, BlogPostSummaryViewModel>
    {
        private readonly AppDbContext _context;
        private readonly BlogPostBuilder _builder;

        public AddBlogPostCommandHandler(AppDbContext context, BlogPostBuilder builder) 
        {
            _context = context;
            _builder = builder;
        }

        public async Task<BlogPostSummaryViewModel> Handle(AddBlogPostCommand message)
        {
            var blogPost = await CreateBlogPost(message);

            await Save(blogPost);

            return ViewModel(blogPost);
        }

        private async Task<BlogPost> CreateBlogPost(AddBlogPostCommand message) 
            => await _builder.WrittenByCurrentAuthor()
                             .WithContent(message)
                             .WithImage(message.ImageFile)
                             .WithTags(message.Tags)
                             .WithExternalLinks(message.ExternalLinks)
                             .Build();

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