using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Builders;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
using CoolBytes.Services;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.Handlers;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.WebAPI.Features.BlogPosts.Profiles;
using CoolBytes.WebAPI.Features.Images.Profiles;
using CoolBytes.WebAPI.Features.Images.Profiles.Resolvers;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class AddBlogPostsTests : TestBase<TestContext>
    {
        private IUserService _userService;
        private AuthorService _authorService;

        public AddBlogPostsTests(TestContext testContext) : base(testContext)
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
                _userService = userService.Object;
                _authorService = new AuthorService(_userService, Context);
            }
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_AddsBlog()
        {
            var imageService = TestContext.CreateImageService();
            var builder = new BlogPostBuilder(_authorService, imageService);
            var handlerContext = TestContext.CreateHandlerContext<BlogPostSummaryViewModel>(CreateMapper());
            var addBlogPostCommandHandler = new AddBlogPostCommandHandler(handlerContext, builder);
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
            var mapper = CreateMapper();
            var imageFactory = TestContext.CreateImageService();
            var builder = new BlogPostBuilder(_authorService, imageFactory);
            var handlerContext = TestContext.CreateHandlerContext<BlogPostSummaryViewModel>(mapper);
            var handler = new AddBlogPostCommandHandler(handlerContext, builder);
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

        private IMapper CreateMapper()
        {
            var sp = TestContext.ServiceProviderBuilder.Add(s =>
                s.AddTransient<IImageViewModelFactory, LocalImageViewModelFactory>()
                    .AddTransient<ImageViewModelResolver>()).Build();
            var profiles = new Profile[] { new BlogPostSummaryViewModelProfile(), new ImageViewModelProfile() };
            var mapper = TestContext.CreateMapper(profiles, sp);
            return mapper;
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