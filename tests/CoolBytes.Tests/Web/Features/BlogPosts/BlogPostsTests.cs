using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts;
using Xunit;

namespace CoolBytes.Tests.Web.Features.BlogPosts
{
    public class BlogPostsTests : IClassFixture<Fixture>
    {
        private readonly AppDbContext _appDbContext;

        public BlogPostsTests(Fixture fixture)
        {
            _appDbContext = fixture.Context;
            _appDbContext.BlogPosts.RemoveRange(_appDbContext.BlogPosts.ToArray());
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
        public async Task GetBlogPostQueryHandler_ReturnsCourse()
        {
            var blogPostId = _appDbContext.BlogPosts.First().Id;
            var blogPostQueryHandler = new GetBlogPostQueryHandler(_appDbContext);

            var result = await blogPostQueryHandler.Handle(new GetBlogPostQuery() {Id = blogPostId});

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddBlogPostCommandHandler_AddsCourse()
        {
            var addBlogPostCommand = new AddBlogPostCommand()
            {
                Subject = "Test",
                ContentIntro = "Test",
                Content = "Test",
                AuthorId = _appDbContext.Authors.First().Id
            };
            var addBlogPostCommandHandler = new AddBlogPostCommandHandler(_appDbContext);

            var result = await addBlogPostCommandHandler.Handle(addBlogPostCommand);

            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task UpdateBlogPostCommandHandler_UpdatesCourse()
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
        public async Task DeleteBlogPostCommandHandler_DeletesCourse()
        {
            var blogPost = _appDbContext.BlogPosts.First();
            var deleteBlogPostCommand = new DeleteBlogPostCommand() {Id = blogPost.Id };
            var deleteBlogPostCommandHandler = new DeleteBlogPostCommandHandler(_appDbContext);

            await deleteBlogPostCommandHandler.Handle(deleteBlogPostCommand);

            Assert.Null(await _appDbContext.BlogPosts.FindAsync(blogPost.Id));
        }

        private void SeedDb()
        {
            var author = new Author("Tom", "Bina", "About me");
            var blogPost = new BlogPost("Testsubject", "Testintro", "Testcontent", author);
            _appDbContext.BlogPosts.Add(blogPost);
            _appDbContext.SaveChanges();
        }
    }
}
