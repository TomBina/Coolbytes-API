using System;
using System.IO;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts;
using CoolBytes.WebAPI.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Factories;
using CoolBytes.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class BlogPostsTests : IClassFixture<Fixture>, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly Fixture _fixture;

        public BlogPostsTests(Fixture fixture)
        {
            _fixture = fixture;
            _appDbContext = fixture.GetNewContext();
            _userService = fixture.UserService;

            SeedDb();
        }

        [Fact]
        public async Task GetBlogPostsQueryHandler_ReturnsCourses()
        {
            var blogPostsQueryHandler = new GetBlogPostsQueryHandler(_appDbContext);

            var result = await blogPostsQueryHandler.Handle(new GetBlogPostsQuery());

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetBlogPostQueryHandler_ReturnsBlog()
        {
            var blogPostId = _appDbContext.BlogPosts.First().Id;
            var blogPostQueryHandler = new GetBlogPostQueryHandler(_appDbContext);

            var result = await blogPostQueryHandler.Handle(new GetBlogPostQuery() { Id = blogPostId });

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_AddsBlog()
        {
            var addBlogPostCommand = new AddBlogPostCommand()
            {
                Subject = "Test",
                ContentIntro = "Test",
                Content = "Test",
                AuthorId = _appDbContext.Authors.First().Id
            };

            var addBlogPostCommandHandler = new AddBlogPostCommandHandler(_appDbContext, _userService);

            var result = await addBlogPostCommandHandler.Handle(addBlogPostCommand);

            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_WithFile_AddsBlog()
        {
            var options = new PhotoFactoryOptions(Environment.CurrentDirectory);
            var validator = new PhotoFactoryValidator();
            var photoFactory = new PhotoFactory(options, validator);

            var handler = new AddBlogPostCommandHandler(_appDbContext, _userService, photoFactory);
            var fileMock = CreateFileMock();
            var file = fileMock.Object;

            var message = new AddBlogPostCommand()
            {
                Subject = "Test",
                Content = "Test",
                ContentIntro = "Test",
                File = file,
                AuthorId = _appDbContext.Authors.First().Id
            };

            var result = await handler.Handle(message);

            Assert.NotNull(result.Photo);
        }

        private Mock<IFormFile> CreateFileMock()
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(e => e.FileName).Returns("testimage.png");
            mock.Setup(e => e.ContentType).Returns("image/png");
            mock.Setup(e => e.OpenReadStream()).Returns(() => File.Open("assets/testimage.png", FileMode.Open));
            return mock;
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_UpdatesBlog()
        {
            var blogPost = _appDbContext.BlogPosts.First();
            var updateBlogPostCommand = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test",
                AuthorId = blogPost.AuthorId
            };
            var updateBlogPostCommandHandler = new UpdateBlogPostCommandHandler(_appDbContext);

            await updateBlogPostCommandHandler.Handle(updateBlogPostCommand);

            Assert.Equal("Test new", blogPost.Subject);
        }

        [Fact]
        public async Task DeleteBlogPostCommandHandler_DeletesBlog()
        {
            var blogPost = _appDbContext.BlogPosts.First();
            var deleteBlogPostCommand = new DeleteBlogPostCommand() { Id = blogPost.Id };
            var deleteBlogPostCommandHandler = new DeleteBlogPostCommandHandler(_appDbContext);

            await deleteBlogPostCommandHandler.Handle(deleteBlogPostCommand);

            Assert.Null(await _appDbContext.BlogPosts.FindAsync(blogPost.Id));
        }

        private void SeedDb()
        {
            using (var context = _fixture.GetNewContext())
            {
                var user = new User("Test");
                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorData = new AuthorData(_appDbContext);
                var author = Author.Create(user, authorProfile, authorData).Result;
                var blogPost = new BlogPost("Testsubject", "Testintro", "Testcontent", author);

                context.BlogPosts.Add(blogPost);
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _appDbContext.BlogPosts.RemoveRange(_appDbContext.BlogPosts.ToArray());
            _appDbContext.Authors.RemoveRange(_appDbContext.Authors.ToArray());
            _appDbContext.Users.RemoveRange(_appDbContext.Users.ToArray());
            _appDbContext.SaveChanges();
            _appDbContext.Dispose();
        }
    }
}
