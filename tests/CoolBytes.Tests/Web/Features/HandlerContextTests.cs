using CoolBytes.WebAPI.Handlers;
using Xunit;

namespace CoolBytes.Tests.Web.Features
{
    public class HandlerContextTests : TestBase<TestContext>
    {
        public HandlerContextTests(TestContext testContext) : base(testContext)
        {
        }

        [Fact]
        public void HandlerContext_Maps_Correctly()
        {
            var mapper = TestContext.CreateMapper(new[] { new TestViewModelProfile() });
            var cacheService = TestContext.CreateStubCacheService();
            var context = new HandlerContext<TestViewModel>(mapper, Context, cacheService);

            var testModel = new TestModel() { FirstName = "Test", LastName = "Test" };
            var testViewModel = context.Map(testModel);

            Assert.Equal("Test Test", testViewModel.FullName);
        }
    }
}
