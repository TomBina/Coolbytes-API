using CoolBytes.Core.Builders;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.Handlers;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class BlogPostsTests : TestBase
    {
        private IUserService _userService;
        private AuthorService _authorService;

        public BlogPostsTests(TestContext testContext) : base(testContext)
        {
        }

        public override async Task InitializeAsync()
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

                var userService = new Mock<IUserService>();
                userService.Setup(exp => exp.GetOrCreateCurrentUserAsync()).ReturnsAsync(user);
                userService.Setup(exp => exp.TryGetCurrentUserAsync()).ReturnsAsync(user.ToSuccessResult());
                _userService = userService.Object;
                _authorService = new AuthorService(_userService, Context);
            }
        }

        private async Task<BlogPost> AddBlog()
        {
            using (var context = TestContext.CreateNewContext())
            {
                var blogPostContent = new BlogPostContent("Testsubject", "Testintro", "Testcontent");
                var category = new Category("Testcategory");
                var author = await _authorService.GetAuthor();
                var blogPost = new BlogPost(blogPostContent, author, category);

                context.Entry(author).State = EntityState.Unchanged;
                context.BlogPosts.Add(blogPost);
                await context.SaveChangesAsync();

                return blogPost;
            }
        }

        [Fact]
        public async Task GetBlogPostsQueryHandler_ReturnsBlogs()
        {
            var blogPostsQueryHandler = new GetBlogPostsQueryHandler(Context, TestContext.CreateStubCacheService());
            var getBlogPostsQuery = new GetBlogPostsQuery();
            var result = await blogPostsQueryHandler.Handle(getBlogPostsQuery, CancellationToken.None);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetBlogPostsByCategoryQueryHandler_ReturnsBlogs()
        {
            var query = new GetBlogPostsByCategoryQuery();
            using (var context = TestContext.CreateNewContext())
            {
                var category = await context.Categories.FirstAsync();
                query.CategoryId = category.Id;
            }
            var handler = new GetBlogPostsByCategoryQueryHandler(Context, TestContext.CreateStubCacheService());
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetBlogPostsOverviewQueryHandler_ReturnsBlogs()
        {
            var blogPostsQueryHandler = new GetBlogPostsOverviewQueryHandler(Context, TestContext.CreateStubCacheService());

            var result = await blogPostsQueryHandler.Handle(new GetBlogPostsOverviewQuery(), CancellationToken.None);

            Assert.NotEmpty(result.Categories);
        }

        [Fact]
        public async Task GetBlogPostQueryHandler_ReturnsBlog()
        {
            var blogPostId = Context.BlogPosts.First().Id;
            var blogPostQueryHandler = new GetBlogPostQueryHandler(Context, TestContext.CreateStubCacheService());

            var result = await blogPostQueryHandler.Handle(new GetBlogPostQuery() { Id = blogPostId }, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_AddsBlog()
        {
            var imageFactory = TestContext.CreateImageFactory();
            var builder = new BlogPostBuilder(_authorService, imageFactory);
            var addBlogPostCommandHandler = new AddBlogPostCommandHandler(Context, builder);
            var category = await Context.Categories.FirstOrDefaultAsync();
            var addBlogPostCommand = new AddBlogPostCommand()
            {
                Subject = "Test",
                ContentIntro = "Test",
                Content = "Test",
                CategoryId = category.Id
            };

            var result = await addBlogPostCommandHandler.Handle(addBlogPostCommand, CancellationToken.None);

            Assert.InRange(result.Id, 1, int.MaxValue);
            Assert.Equal(category.Name, result.Category);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_WithFile_AddsBlog()
        {
            var imageFactory = TestContext.CreateImageFactory();
            var builder = new BlogPostBuilder(_authorService, imageFactory);
            var handler = new AddBlogPostCommandHandler(Context, builder);
            var fileMock = TestContext.CreateFileMock();
            var file = fileMock.Object;
            var category = await Context.Categories.FirstOrDefaultAsync();

            var message = new AddBlogPostCommand()
            {
                Subject = "Test",
                Content = "Test",
                ContentIntro = "Test",
                File = file,
                CategoryId = category.Id
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
    }
}