using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Factories;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class UpdateBlogPostCommandHandler : IAsyncRequestHandler<UpdateBlogPostCommand, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IImageFactory _imageFactory;
        private bool _isImageUpdated;

        public UpdateBlogPostCommandHandler(AppDbContext appDbContext, IImageFactory imageFactory)
        {
            _appDbContext = appDbContext;
            _imageFactory = imageFactory;
        }

        public async Task<BlogPostViewModel> Handle(UpdateBlogPostCommand message)
        {
            var blogPost = await GetBlogPost(message);

            await UpdateBlogPost(message, blogPost);
            await SaveBlogPost(blogPost);

            return CreateViewModel(blogPost);
        }

        private async Task<BlogPost> GetBlogPost(UpdateBlogPostCommand message)
        {
            var blogPost = await _appDbContext.BlogPosts.Include(b => b.Tags).SingleOrDefaultAsync(b => b.Id == message.Id);
            return blogPost;
        }

        private async Task UpdateBlogPost(UpdateBlogPostCommand message, BlogPost blogPost)
        {
            blogPost.Update(message.Subject, message.ContentIntro, message.Content);

            if (message.Tags != null)
                blogPost.UpdateTags(message.Tags.Select(s => new BlogPostTag(s)));

            if (message.File == null)
                return;

            var image = await CreateImage(message.File);
            blogPost.SetImage(image);
        }

        private async Task<Image> CreateImage(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var image = await _imageFactory.Create(stream, file.FileName, file.ContentType);
                _isImageUpdated = true;
                return image;
            }
        }

        private async Task SaveBlogPost(BlogPost blogPost)
        {
            if (_isImageUpdated)
                await _appDbContext.SaveChangesAsync(() => File.Delete(blogPost.Image.Path));
            else
                await _appDbContext.SaveChangesAsync();
        }

        private BlogPostViewModel CreateViewModel(BlogPost blogPost) => Mapper.Map<BlogPostViewModel>(blogPost);
    }
}