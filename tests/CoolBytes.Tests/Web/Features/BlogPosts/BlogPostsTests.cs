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
    public class BlogPostsTests : IClassFixture<Fixture>, IAsyncLifetime
    {
        private readonly AppDbContext _appDbContext;
        private IUserService _userService;
        private readonly Fixture _fixture;

        public BlogPostsTests(Fixture fixture)
        {
            _fixture = fixture;
            _appDbContext = fixture.GetNewContext();
        }

        [Fact]
        public async Task GetBlogPostsQueryHandler_ReturnsCourses()
        {
            var blogPostsQueryHandler = new GetBlogPostsQueryHandler(_appDbContext, _fixture.Configuration);

            var result = await blogPostsQueryHandler.Handle(new GetBlogPostsQuery());

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetBlogPostQueryHandler_ReturnsBlog()
        {
            var blogPostId = _appDbContext.BlogPosts.First().Id;
            var blogPostQueryHandler = new GetBlogPostQueryHandler(_appDbContext, _fixture.Configuration);

            var result = await blogPostQueryHandler.Handle(new GetBlogPostQuery() { Id = blogPostId });

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_AddsBlog()
        {
            var photoFactory = GetPhotoFactory();
            var addBlogPostCommandHandler = new AddBlogPostCommandHandler(_appDbContext, _userService, photoFactory, _fixture.Configuration);
            var addBlogPostCommand = new AddBlogPostCommand()
            {
                Subject = "Test",
                ContentIntro = "Test",
                Content = "Test"
            };

            var result = await addBlogPostCommandHandler.Handle(addBlogPostCommand);

            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_WithFile_AddsBlog()
        {
            var photoFactory = GetPhotoFactory();
            var handler = new AddBlogPostCommandHandler(_appDbContext, _userService, photoFactory, _fixture.Configuration);
            var fileMock = CreateFileMock();
            var file = fileMock.Object;

            var message = new AddBlogPostCommand()
            {
                Subject = "Test",
                Content = "Test",
                ContentIntro = "Test",
                File = file
            };

            var result = await handler.Handle(message);

            Assert.NotNull(result.Photo.PhotoUri);
        }

        private static PhotoFactory GetPhotoFactory()
        {
            var options = new PhotoFactoryOptions(Environment.CurrentDirectory);
            var validator = new PhotoFactoryValidator();
            var photoFactory = new PhotoFactory(options, validator);
            return photoFactory;
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
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test"
            };
            var updateBlogPostCommandHandler = new UpdateBlogPostCommandHandler(_appDbContext, _userService, null, _fixture.Configuration);

            await updateBlogPostCommandHandler.Handle(message);

            Assert.Equal("Test new", blogPost.Subject);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_WithFile_UpdatesBlog()
        {
            var photoFactory = GetPhotoFactory();
            var handler = new UpdateBlogPostCommandHandler(_appDbContext, _userService, photoFactory, _fixture.Configuration);
            var fileMock = CreateFileMock();
            var file = fileMock.Object;

            var blogPost = _appDbContext.BlogPosts.First();
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test",
                File = file
            };

            var result = await handler.Handle(message);

            Assert.NotNull(result.Photo.PhotoUri);
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

        public async Task InitializeAsync() => await SeedDb();

        private async Task SeedDb()
        {
            using (var context = _fixture.GetNewContext())
            {
                var user = new User("Test");

                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(_appDbContext);
                var author = Author.Create(user, authorProfile, authorValidator).Result;
                var blogPost = new BlogPost("Testsubject", "Testintro", "Testcontent", author);

                context.BlogPosts.Add(blogPost);
                await context.SaveChangesAsync();

                var userService = new Mock<IUserService>();
                userService.Setup(exp => exp.GetUser()).ReturnsAsync(user);
                _userService = userService.Object;
            }
        }

        public async Task DisposeAsync()
        {
            _appDbContext.BlogPosts.RemoveRange(_appDbContext.BlogPosts.ToArray());
            _appDbContext.Authors.RemoveRange(_appDbContext.Authors.ToArray());
            _appDbContext.Users.RemoveRange(_appDbContext.Users.ToArray());
            await _appDbContext.SaveChangesAsync();
            _appDbContext.Dispose();
        }
    }
}
