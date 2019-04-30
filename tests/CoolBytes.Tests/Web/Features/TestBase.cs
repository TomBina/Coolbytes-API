using CoolBytes.Data;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features
{
    /// <summary>
    /// Provides initialization for each unique test.
    /// </summary>
    public abstract class TestBase<T> : IClassFixture<T>, IAsyncLifetime where T : TestContext
    {
        protected readonly AppDbContext Context;
        protected readonly T TestContext;

        protected TestBase(T testContext)
        {
            TestContext = testContext;
            Context = testContext.CreateNewContext();
        }

        /// <summary>
        /// Runs before each test.
        /// </summary>
        /// <returns>Task</returns>
        public virtual Task InitializeAsync()
            => Task.CompletedTask;

        /// <summary>
        /// Runs after each test.
        /// </summary>
        /// <returns></returns>
        public virtual Task DisposeAsync()
        {
            Context?.Dispose();

            return Task.CompletedTask;
        }
    }
}