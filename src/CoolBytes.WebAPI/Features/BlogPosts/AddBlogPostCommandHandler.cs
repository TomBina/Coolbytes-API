using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

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
            var user = await _userService.GetUser();
            var author = _appDbContext.Authors.Find(message.AuthorId);
            var blogPost = new BlogPost(message.Subject, message.ContentIntro, message.Content, author);

            if (message.Tags != null)
            {
                var blogPostTags = message.Tags?.Select(b => new BlogPostTag(b));
                blogPost.AddTags(blogPostTags);
            }

            if (message.File != null)
            {
                var photo = await CreatePhoto(message.File);
                blogPost.SetPhoto(photo);
            }
            
            _appDbContext.BlogPosts.Add(blogPost);
            await _appDbContext.SaveChangesAsync();

            return Mapper.Map<BlogPostViewModel>(blogPost);
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