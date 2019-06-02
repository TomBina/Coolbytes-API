using System.Collections.Generic;
using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.Handlers;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Categories
{
    public class CategoriesTests : TestBase<TestContext>
    {
        public CategoriesTests(TestContext testContext) : base(testContext)
        {

        }

        public override async Task InitializeAsync()
        {
            using (var context = TestContext.CreateNewContext())
            {
                context.Categories.Add(new  Category("Default category", 1, "Test description"));
                context.Categories.Add(new Category("Another category", 2, "Test description"));

                await context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetAllCategoriesHandler_Returns_Categories()
        {
            var message = new GetAllCategoriesQuery();
            var handlerContext = TestContext.CreateHandlerContext<IEnumerable<CategoryViewModel>>(RequestDbContext);
            var handler = new GetAllCategoriesQueryHandler(handlerContext);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotEmpty(result.Payload);
        }

        [Fact]
        public async Task GetCategoryHandler_Returns_Category()
        {
            var category = await GetRandomCategory();
            var message = new GetCategoryQuery() { Id = category.Id };
            var handlerContext = TestContext.CreateHandlerContext<CategoryViewModel>(RequestDbContext);
            var handler = new GetCategoryQueryHandler(handlerContext);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal(category.Id, result.Payload.Id);
        }

        private async Task<Category> GetRandomCategory()
        {
            Category category;
            using (var context = TestContext.CreateNewContext())
            {
                category = await context.Categories.FirstOrDefaultAsync();
            }

            return category;
        }

        [Fact]
        public async Task AddCategoryHandler_Adds_Category()
        {
            var message = new AddCategoryCommand() { Name = "Test category", Description = "Some description" };
            var handler = new AddCategoryCommandHandler(RequestDbContext);

            var result = await handler.Handle(message, CancellationToken.None);
            
            Assert.IsType<SuccessResult>(result);
        }

        [Fact]
        public async Task UpdateCategoryHandler_Updates_Category()
        {
            var category = await GetRandomCategory();
            var message = new UpdateCategoryCommand() { Id = category.Id, Name = "New name" };
            var handler = new UpdateCategoryCommandHandler(RequestDbContext);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.IsType<SuccessResult>(result);
        }

        [Fact]
        public async Task DeleteCategoryHandler_Deletes_Category()
        {
            var category = await GetRandomCategory();
            var message = new DeleteCategoryCommand() { Id = category.Id };
            var handler = new DeleteCategoryCommandHandler(RequestDbContext);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.IsType<SuccessResult>(result);
        }

        public override async Task DisposeAsync()
        {
            using (var context = TestContext.CreateNewContext())
            {
                var categories = await context.Categories.ToListAsync();
                context.Categories.RemoveRange(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}
