using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
using CoolBytes.Tests.Web.Features;
using CoolBytes.WebAPI.Common;
using FluentValidation.Validators;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web.Common
{
    public class SortValidatorTests : TestBase<TestContext>
    {
        private readonly TestContext _testContext;

        public SortValidatorTests(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        [Fact]
        public async Task Validation_Passed()
        {
            var context = _testContext.CreateNewContext();
            context.BlogPosts.Add(await CreateBlog(1));
            context.BlogPosts.Add(await CreateBlog(2));
            context.BlogPosts.Add(await CreateBlog(3));

            var validator = new SortValidator<BlogPost>(context);
            var newSortOrder = new List<int>() { 1, 2, 3 };

            var result = await validator.Validate(newSortOrder);

            Assert.IsType<SuccessResult>(result);
        }

        private async Task<BlogPost> CreateBlog(int id)
        {
            var author = await Author.Create(new User(""), new AuthorProfile("", "", ""), Mock.Of<IAuthorValidator>());
            var blogPost = new BlogPost(new BlogPostContent("", "", ""), author, new Category("", 1, ""));

            return blogPost;
        }
    }
}
