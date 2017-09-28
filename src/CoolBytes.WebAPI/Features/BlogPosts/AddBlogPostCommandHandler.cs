using System;
using System.IO;
using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class AddBlogPostCommandHandler : IAsyncRequestHandler<AddBlogPostCommand, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly IPhotoFactory _photoFactory;

        public AddBlogPostCommandHandler(AppDbContext appDbContext, IUserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
        }

        public AddBlogPostCommandHandler(AppDbContext appDbContext, IUserService userService, IPhotoFactory photoFactory) : this(appDbContext, userService)
        {
            _photoFactory = photoFactory;
        }

        public async Task<BlogPostViewModel> Handle(AddBlogPostCommand message)
        {
            var blogPost = await CreateBlogPost(message);
            await SaveBlogPost(message, blogPost);
            
            return Mapper.Map<BlogPostViewModel>(blogPost);
        }

        private async Task<BlogPost> CreateBlogPost(AddBlogPostCommand message)
        {
            var user = await _userService.GetUser();
            var author = await _appDbContext.Authors.FirstOrDefaultAsync(a => a.UserId == user.Id);
            var blogPost = new BlogPost(message.Subject, message.ContentIntro, message.Content, author);

            if (message.Tags != null)
            {
                var blogPostTags = message.Tags?.Select(b => new BlogPostTag(b));
                blogPost.AddTags(blogPostTags);
            }

            if (message.File == null)
                return blogPost;

            var photo = await CreatePhoto(message.File);
            blogPost.SetPhoto(photo);
            return blogPost;
        }

        private async Task SaveBlogPost(AddBlogPostCommand message, BlogPost blogPost)
        {
            _appDbContext.BlogPosts.Add(blogPost);

            if (message.File != null)
            {
                try
                {
                    await _appDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    File.Delete(blogPost.Photo.Path);
                    throw;
                }
            }
            else
            {
                await _appDbContext.SaveChangesAsync();
            }
        }

        private async Task<Photo> CreatePhoto(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                return await _photoFactory.Create(stream, file.FileName, file.ContentType);
            }
        }
    }
}