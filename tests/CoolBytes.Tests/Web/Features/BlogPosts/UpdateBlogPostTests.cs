﻿using CoolBytes.Core.Builders;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.Handlers;
using CoolBytes.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                var category = new Category("Testcategory");
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
            var handler = new UpdateBlogPostQueryHandler(Context);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_UpdatesBlog()
        {
            var category = new Category("Hello test category");
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
            var handler = new UpdateBlogPostCommandHandler(Context, builder);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal("Test new", result.Subject);
            Assert.Equal(category.Name, result.Category);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_WithFile_UpdatesBlog()
        {
            var imageFactory = TestContext.CreateImageFactory();
            var builder = new ExistingBlogPostBuilder(imageFactory);
            var handler = new UpdateBlogPostCommandHandler(Context, builder);
            var fileMock = TestContext.CreateFileMock();
            var file = fileMock.Object;
            var category = new Category("Hello test category");
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