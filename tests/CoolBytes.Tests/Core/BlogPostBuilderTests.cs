using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Builders;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Core
{
    public class BlogPostBuilderTests
    {
        [Fact]
        public async Task BlogPostBuilder_BuildsBlogWithMetaTag()
        {
            var author = Author.Create(new User(""), new AuthorProfile("", "", ""), Mock.Of<IAuthorValidator>());
            var mock = new Mock<IAuthorService>();
            mock.Setup(a => a.GetAuthor()).Returns(author);
            var authorService = mock.Object;
            var imageService = new Mock<IImageService>().Object;
            var builder = new BlogPostBuilder(authorService, imageService);
            var tags = new List<MetaTag>()
            {
                new MetaTag("description", "test description")
            };
            var addBlogPostCommand = new AddBlogPostCommand()
            {
                ContentIntro = "Intro",
                Content = "Test",
                Subject = "Subject"
            };
            var category = new Category("", 1, "");
            var blogPost = await builder.WrittenByCurrentAuthor().WithCategory(category).WithContent(addBlogPostCommand)
                .WithMetaTags(tags).Build();

            Assert.Single(blogPost.MetaTags);
        }

        [Fact]
        public async Task BlogPostBuilder_DoesNotUpdatesBlogWithEqualMetaTag()
        {
            var blogPostContent = new BlogPostContent("", "", "");
            var author = await Author.Create(new User(""), new AuthorProfile("", "", ""), Mock.Of<IAuthorValidator>());
            var category = new Category("", 1, "");
            var blogPost = new BlogPost(blogPostContent, author, category);

            blogPost.MetaTags.Add(new MetaTag("description", "test description"));
            var builder = new ExistingBlogPostBuilder(Mock.Of<IImageService>());
            var tags = new[]
            {
                new MetaTag("description", "test description")
            };
            await builder.UseBlogPost(blogPost).WithMetaTags(tags).Build();

            Assert.Single(blogPost.MetaTags);
        }
    }
}
