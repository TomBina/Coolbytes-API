using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Builders;
using CoolBytes.Core.Factories;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class UpdateBlogPostCommandHandler : IAsyncRequestHandler<UpdateBlogPostCommand, BlogPostSummaryViewModel>
    {
        private readonly AppDbContext _context;
        private readonly ExistingBlogPostBuilder _builder;
        private int? _currentImageId;

        public UpdateBlogPostCommandHandler(AppDbContext context, ExistingBlogPostBuilder builder)
        {
            _context = context;
            _builder = builder;
        }

        public async Task<BlogPostSummaryViewModel> Handle(UpdateBlogPostCommand message)
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
                                         .SingleOrDefaultAsync(b => b.Id == blogPostId);

            _currentImageId = blogPost.ImageId;

            return blogPost;
        }

        private async Task UpdateBlogPost(BlogPost blogPost, UpdateBlogPostCommand message) 
            => await _builder.UseBlogPost(blogPost)
                             .WithContent(message)
                             .WithImage(message.ImageFile)
                             .WithTags(message.Tags)
                             .Build();

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