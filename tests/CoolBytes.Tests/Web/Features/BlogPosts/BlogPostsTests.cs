using CoolBytes.Core.Builders;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.Handlers;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class BlogPostsTests : TestBase, IClassFixture<TestContext>, IAsyncLifetime
    {
        public BlogPostsTests(TestContext testContext) : base(testContext)
        {
        }

        public async Task InitializeAsync() => await SeedDb();

        private async Task SeedDb()
        {
            using (var context = TestContext.CreateNewContext())
            {
                var user = new User("Test");

                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(Context);
                var author = await Author.Create(user, authorProfile, authorValidator);
                var blogPostContent = new BlogPostContent("Testsubject", "Testintro", "Testcontent");
                var category = new Category("Testcategory");
                var blogPost = new BlogPost(blogPostContent, author, category);

                context.BlogPosts.Add(blogPost);
                await context.SaveChangesAsync();

                InitUserService(user);
                InitAuthorService();
            }
        }

        [Fact]
        public async Task GetBlogPostsQueryHandler_ReturnsBlogs()
        {
            var blogPostsQueryHandler = new GetBlogPostsQueryHandler(Context, TestContext.CacheService());

            var result = await blogPostsQueryHandler.Handle(new GetBlogPostsQuery(), CancellationToken.None);

            Assert.NotEmpty(result.Categories);
        }

        [Fact]
        public async Task GetBlogPostQueryHandler_ReturnsBlog()
        {
            var blogPostId = Context.BlogPosts.First().Id;
            var blogPostQueryHandler = new GetBlogPostQueryHandler(Context, TestContext.CacheService());

            var result = await blogPostQueryHandler.Handle(new GetBlogPostQuery() { Id = blogPostId }, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_AddsBlog()
        {
            var imageFactory = CreateImageFactory();
            var builder = new BlogPostBuilder(AuthorService, imageFactory);

            var addBlogPostCommandHandler = new AddBlogPostCommandHandler(Context, builder);
            var addBlogPostCommand = new AddBlogPostCommand()
            {
                Subject = "Test",
                ContentIntro = "Test",
                Content = "Test"
            };

            var result = await addBlogPostCommandHandler.Handle(addBlogPostCommand, CancellationToken.None);

            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_WithFile_AddsBlog()
        {
            var imageFactory = CreateImageFactory();
            var builder = new BlogPostBuilder(AuthorService, imageFactory);
            var handler = new AddBlogPostCommandHandler(Context, builder);
            var fileMock = CreateFileMock();
            var file = fileMock.Object;

            var message = new AddBlogPostCommand()
            {
                Subject = "Test",
                Content = "Test",
                ContentIntro = "Test",
                File = file
            };

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result.Image.UriPath);
        }

        [Fact]
        public async Task UpdateBlogPostQueryHandler_ReturnsBlogAsync()
        {
            var blog = Context.BlogPosts.First();
            var query = new UpdateBlogPostQuery() { Id = blog.Id };
            var handler = new UpdateBlogPostQueryHandler(Context);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_UpdatesBlog()
        {
            var blogPost = Context.BlogPosts.AsNoTracking().First();
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test"
            };

            var builder = new ExistingBlogPostBuilder(null);
            var handler = new UpdateBlogPostCommandHandler(Context, builder);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal("Test new", result.Subject);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_WithFile_UpdatesBlog()
        {
            var imageFactory = CreateImageFactory();
            var builder = new ExistingBlogPostBuilder(imageFactory);
            var handler = new UpdateBlogPostCommandHandler(Context, builder);
            var fileMock = CreateFileMock();
            var file = fileMock.Object;

            var blogPost = Context.BlogPosts.AsNoTracking().First();
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test",
                File = file
            };

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result.Image.UriPath);
        }

        [Fact]
        public async Task DeleteBlogPostCommandHandler_DeletesBlog()
        {
            var blogPost = Context.BlogPosts.First();
            var deleteBlogPostCommand = new DeleteBlogPostCommand() { Id = blogPost.Id };
            IRequestHandler<DeleteBlogPostCommand> deleteBlogPostCommandHandler = new DeleteBlogPostCommandHandler(Context);

            await deleteBlogPostCommandHandler.Handle(deleteBlogPostCommand, CancellationToken.None);

            Assert.Null(await Context.BlogPosts.FindAsync(blogPost.Id));
        }

        public async Task DisposeAsync()
        {
            Context.Dispose();

            await Task.CompletedTask;
        }
    }
}
