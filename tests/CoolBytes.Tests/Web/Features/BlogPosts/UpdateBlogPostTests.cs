using CoolBytes.Core.Builders;
using CoolBytes.Core.Utils;
using CoolBytes.Services;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.Handlers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class UpdateBlogPostTests : TestBase
    {
        public UpdateBlogPostTests(TestContext testContext) : base(testContext)
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
                var category = new Category("Testcategory", 1);
                var blogPost = new BlogPost(blogPostContent, author, category);

                context.BlogPosts.Add(blogPost);
                await context.SaveChangesAsync();

                var userService = new Mock<IUserService>();
                userService.Setup(exp => exp.GetOrCreateCurrentUserAsync()).ReturnsAsync(user);
                userService.Setup(exp => exp.TryGetCurrentUserAsync()).ReturnsAsync(user.ToSuccessResult());
            }
        }

        [Fact]
        public async Task UpdateBlogPostQueryHandler_ReturnsBlogAsync()
        {
            var blog = await Context.BlogPosts.FirstAsync();
            var query = new UpdateBlogPostQuery() { Id = blog.Id };
            var handler = new UpdateBlogPostQueryHandler(TestContext.CreateHandlerContext<BlogPostUpdateViewModel>());

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_UpdatesBlog()
        {
            var category = new Category("Hello test category", 1);
            using (var context = TestContext.CreateNewContext())
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
            }
            var blogPost = Context.BlogPosts.AsNoTracking().First();
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test",
                CategoryId = category.Id
            };
            var builder = new ExistingBlogPostBuilder(null);
            var handler = new UpdateBlogPostCommandHandler(TestContext.CreateHandlerContext<BlogPostSummaryViewModel>(), builder);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal("Test new", result.Subject);
            Assert.Equal(category.Name, result.Category);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_WithFile_UpdatesBlog()
        {
            var imageFactory = TestContext.CreateImageService();
            var builder = new ExistingBlogPostBuilder(imageFactory);
            var handler = new UpdateBlogPostCommandHandler(TestContext.CreateHandlerContext<BlogPostSummaryViewModel>(), builder);
            var fileMock = TestContext.CreateFileMock();
            var file = fileMock.Object;
            var category = new Category("Hello test category", 1);
            using (var context = TestContext.CreateNewContext())
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
            }

            var blogPost = Context.BlogPosts.AsNoTracking().First();
            var message = new UpdateBlogPostCommand()
            {
                Id = blogPost.Id,
                Subject = "Test new",
                ContentIntro = "Test",
                Content = "Test",
                File = file,
                CategoryId = category.Id
            };

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result.Image.UriPath);
        }
    }
}
