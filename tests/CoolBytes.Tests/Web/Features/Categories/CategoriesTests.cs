using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.Handlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Categories
{
    public class CategoriesTests : TestBase
    {
        public CategoriesTests(TestContext testContext) : base(testContext)
        {

        }

        public override async Task InitializeAsync()
        {
            using (var context = TestContext.CreateNewContext())
            {
                context.Categories.Add(new Category("Default category"));
                context.Categories.Add(new Category("Another category"));

                await context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetAllCategoriesHandler_Returns_Categories()
        {
            var message = new GetAllCategoriesQuery();
            var handler = new GetAllCategoriesHandler(Context);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotEmpty(result.Payload);
        }
    }
}
