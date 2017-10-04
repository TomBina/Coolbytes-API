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
using CoolBytes.Tests.Web.Features.Authors;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class BlogPostsTests : TestBase, IClassFixture<Fixture>, IAsyncLifetime
    {
        public BlogPostsTests(Fixture fixture) : base(fixture)
        {
        }

        public async Task InitializeAsync() => await SeedDb();

        private async Task SeedDb()
        {
            using (var context = Fixture.CreateNewContext())
            {
                var user = new User("Test");

                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(Context);
                var author = Author.Create(user, authorProfile, authorValidator).Result;
                var blogPost = new BlogPost("Testsubject", "Testintro", "Testcontent", author);

                context.BlogPosts.Add(blogPost);
                await context.SaveChangesAsync();

                InitUserService(user);
            }
        }

        [Fact]
        public async Task GetBlogPostsQueryHandler_ReturnsBlogs()
        {
            var blogPostsQueryHandler = new GetBlogPostsQueryHandler(Context);

            var result = await blogPostsQueryHandler.Handle(new GetBlogPostsQuery());

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetBlogPostQueryHandler_ReturnsBlog()
        {
            var blogPostId = Context.BlogPosts.First().Id;
            var blogPostQueryHandler = new GetBlogPostQueryHandler(Context);

            var result = await blogPostQueryHandler.Handle(new GetBlogPostQuery() { Id = blogPostId });

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_AddsBlog()
        {
            var imageFactory = CreateImageFactory();
            var addBlogPostCommandHandler = new AddBlogPostCommandHandler(Context, UserService, imageFactory);
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
            var imageFactory = CreateImageFactory();
            var handler = new AddBlogPostCommandHandler(Context, UserService, imageFactory);
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

            Assert.NotNull(result.Image.UriPath);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_UpdatesBlog()
        {
            var blogPost = Context.BlogPosts.First();
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test"
            };
            var updateBlogPostCommandHandler = new UpdateBlogPostCommandHandler(Context, null);

            await updateBlogPostCommandHandler.Handle(message);

            Assert.Equal("Test new", blogPost.Subject);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_WithFile_UpdatesBlog()
        {
            var imageFactory = CreateImageFactory();
            var handler = new UpdateBlogPostCommandHandler(Context, imageFactory);
            var fileMock = CreateFileMock();
            var file = fileMock.Object;

            var blogPost = Context.BlogPosts.First();
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test",
                File = file
            };

            var result = await handler.Handle(message);

            Assert.NotNull(result.Image.UriPath);
        }

        [Fact]
        public async Task DeleteBlogPostCommandHandler_DeletesBlog()
        {
            var blogPost = Context.BlogPosts.First();
            var deleteBlogPostCommand = new DeleteBlogPostCommand() { Id = blogPost.Id };
            var deleteBlogPostCommandHandler = new DeleteBlogPostCommandHandler(Context);

            await deleteBlogPostCommandHandler.Handle(deleteBlogPostCommand);

            Assert.Null(await Context.BlogPosts.FindAsync(blogPost.Id));
        }
      
        public async Task DisposeAsync()
        {
            Context.BlogPosts.RemoveRange(Context.BlogPosts.ToArray());
            Context.Authors.RemoveRange(Context.Authors.ToArray());
            Context.Users.RemoveRange(Context.Users.ToArray());
            await Context.SaveChangesAsync();
            Context.Dispose();
        }
    }
}
