using CoolBytes.Data;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features
{
    /// <summary>
    /// Provides initialization for each unique test.
    /// </summary>
    public abstract class TestBase : IClassFixture<TestContext>, IAsyncLifetime
    {
        protected readonly AppDbContext Context;
        protected readonly TestContext TestContext;

        protected TestBase(TestContext testContext)
        {
            TestContext = testContext;
            Context = testContext.CreateNewContext();
        }

        public virtual Task InitializeAsync() 
            => Task.CompletedTask;

        public virtual Task DisposeAsync()
        {
            Context?.Dispose();

            return Task.CompletedTask;
        }
    }
}