using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
using CoolBytes.Tests.Web.Features;
using CoolBytes.WebAPI.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Common
{
    public class SortValidatorTests : TestBase<TestContext>
    {
        private readonly TestContext _testContext;
        private List<int> _blogPosts;

        public SortValidatorTests(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
        }

        public override async Task InitializeAsync()
        {
            async Task<BlogPost> CreateBlog()
            {
                var author = await Author.Create(new User(""), new AuthorProfile("", "", ""), Mock.Of<IAuthorValidator>());
                var blogPost = new BlogPost(new BlogPostContent("", "", ""), author, new Category(1, "", 1, ""));
                return blogPost;
            }

            var context = _testContext.CreateNewContext();
            context.BlogPosts.Add(await CreateBlog());
            context.BlogPosts.Add(await CreateBlog());
            context.BlogPosts.Add(await CreateBlog());
            await context.SaveChangesAsync();

            _blogPosts = await context.BlogPosts.Select(b => b.Id).ToListAsync();
        }

        [Fact]
        public async Task Validation_Passes()
        {
            var context = TestContext.CreateNewContext();
            var validator = new SortValidator<BlogPost>(context);
            var newSortOrder = _blogPosts.OrderByDescending(i => i).ToList();

            var result = await validator.Validate(newSortOrder, b => b.CategoryId == 1);

            Assert.IsType<SuccessResult>(result);
        }

        [Fact]
        public async Task Validation_Fails_When_Ids_Count_Does_Not_Match_Db_Ids_Count()
        {
            var context = TestContext.CreateNewContext();
            var validator = new SortValidator<BlogPost>(context);
            var newSortOrder = new List<int>() { _blogPosts[0], _blogPosts[1] };

            var result = await validator.Validate(newSortOrder);

            Assert.IsType<ErrorResult>(result);
        }

        [Fact]
        public async Task Validation_Fails_When_An_Id_Does_Not_Exist()
        {
            var context = TestContext.CreateNewContext();
            var validator = new SortValidator<BlogPost>(context);
            var newSortOrder = new List<int>() { _blogPosts[0], _blogPosts[1], _blogPosts[2], 100 };

            var result = await validator.Validate(newSortOrder);

            Assert.IsType<ErrorResult>(result);
        }

        [Fact]
        public async Task Validation_Fails_When_Are_Not_All_Unique()
        {
            var context = TestContext.CreateNewContext();
            var validator = new SortValidator<Category>(context);
            var newSortOrder = new List<int>() { _blogPosts[0], _blogPosts[1], _blogPosts[0] };

            var result = await validator.Validate(newSortOrder);

            Assert.IsType<ErrorResult>(result);
        }


        public override async Task DisposeAsync()
        {
            using (var context = TestContext.CreateNewContext())
            {
                context.BlogPosts.RemoveRange(await context.BlogPosts.ToListAsync());
                context.Categories.RemoveRange(await context.Categories.ToListAsync());
                await context.SaveChangesAsync();
            }
        }
    }
}
