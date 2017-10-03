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
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class UpdateBlogPostCommandHandler : IAsyncRequestHandler<UpdateBlogPostCommand, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IPhotoFactory _photoFactory;
        private bool _isPhotoUpdated;

        public UpdateBlogPostCommandHandler(AppDbContext appDbContext, IPhotoFactory photoFactory, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _photoFactory = photoFactory;
            _configuration = configuration;
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

            var photo = await CreatePhoto(message.File);
            blogPost.SetPhoto(photo);
        }

        private async Task<Photo> CreatePhoto(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var photo = await _photoFactory.Create(stream, file.FileName, file.ContentType);
                _isPhotoUpdated = true;
                return photo;
            }
        }

        private async Task SaveBlogPost(BlogPost blogPost)
        {
            if (_isPhotoUpdated)
                await _appDbContext.SaveChangesAsync(() => File.Delete(blogPost.Photo.Path));
            else
                await _appDbContext.SaveChangesAsync();
        }

        private BlogPostViewModel CreateViewModel(BlogPost blogPost)
        {
            var viewModel = Mapper.Map<BlogPostViewModel>(blogPost);
            viewModel.Photo?.FormatPhotoUri(_configuration);

            return viewModel;
        }

    }
}