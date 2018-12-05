using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.Handlers;
using CoolBytes.WebAPI.Services.Caching;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Caching
{
    public class CachingTests
    {
        [Fact]
        public async Task TruncateCacheHandler_Truncates_Whole_Cache()
        {
            var command = new TruncateCacheCommand();
            var cacheKeyGenerator = new CacheKeyGenerator();
            var memoryCacheService = new MemoryCacheService(cacheKeyGenerator);
            await memoryCacheService.AddAsync("hello", () => Task.FromResult("hello"));
            var cacheStore = (ConcurrentDictionary<string, object>)typeof(MemoryCacheService).GetField("Store", BindingFlags.NonPublic | BindingFlags.Static).GetValue(memoryCacheService);
            var countBeforeTest = cacheStore.Count;

            var handler = new TruncateCacheCommandHandler(memoryCacheService);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(1, countBeforeTest);
            Assert.IsType<SuccessResult>(result);
            Assert.Empty(cacheStore);
        }
    }
}
