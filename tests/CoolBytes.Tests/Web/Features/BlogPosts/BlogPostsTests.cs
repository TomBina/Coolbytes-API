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
            var blogPostCommand = new AddBlogPostCommand()
            {
                Subject = "Test",
                ContentIntro = "Test",
                Content = "Test",
                AuthorId = _appDbContext.Authors.First().Id
            };
            var blogPostCommandHandler = new AddBlogPostCommandHandler(_appDbContext);

            var result = await blogPostCommandHandler.Handle(blogPostCommand);

            Assert.NotNull(result.Id);
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
