using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.Services;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.Handlers;
using CoolBytes.WebAPI.Features.BlogPosts.Profiles;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Features.Images.Profiles.Resolvers;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class GetBlogsTests : TestBase<TestContext>
    {
        public GetBlogsTests(TestContext testContext) : base(testContext)
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
            }
        }

        private IMapper CreateMapper()
        {
            var sp = TestContext.ServiceProviderBuilder.Add(s =>
                s.AddTransient<IImageViewModelUrlResolver, AzureBlobImageViewModelUrlResolver>()
                    .AddTransient<ImageViewModelResolver>()).Build();
            var profiles = new[] { new BlogPostSummaryViewModelProfile() };
            var mapper = TestContext.CreateMapper(profiles, sp);

            return mapper;
        }

        [Fact]
        public async Task GetBlogPostsQueryHandler_ReturnsBlogs()
        {
            var handlerContext = TestContext.CreateHandlerContext<IEnumerable<BlogPostSummaryViewModel>>(CreateMapper());
            var blogPostsQueryHandler = new GetBlogPostsQueryHandler(handlerContext);
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
            var handlerContext = TestContext.CreateHandlerContext<IEnumerable<BlogPostSummaryViewModel>>(CreateMapper());
            var handler = new GetBlogPostsByCategoryQueryHandler(handlerContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetBlogPostsOverviewQueryHandler_ReturnsBlogs()
        {
            var handlerContext = TestContext.CreateHandlerContext<BlogPostsOverviewViewModel>(CreateMapper());
            var blogPostsQueryHandler = new GetBlogPostsOverviewQueryHandler(handlerContext);

            var result = await blogPostsQueryHandler.Handle(new GetBlogPostsOverviewQuery(), CancellationToken.None);

            Assert.NotEmpty(result.Categories);
        }

        [Fact]
        public async Task GetBlogPostQueryHandler_ReturnsBlog()
        {
            var blogPostId = Context.BlogPosts.First().Id;
            var blogPostQueryHandler = new GetBlogPostQueryHandler(TestContext.CreateHandlerContext<BlogPostViewModel>());

            var result = await blogPostQueryHandler.Handle(new GetBlogPostQuery() { Id = blogPostId }, CancellationToken.None);

            Assert.NotNull(result);
        }

    }
}
