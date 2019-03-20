﻿using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.Handlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Caching
{
    public class CachingTests : TestBase
    {
        public CachingTests(TestContext testContext) : base(testContext)
        {
        }

        [Fact]
        public async Task TruncateCacheHandler_Truncates_Whole_Cache()
        {
            var command = new InvalidateCacheCommand();
            var memoryCacheService = TestContext.CreateMemoryCacheService();
            await memoryCacheService.AddAsync("hello", () => Task.FromResult("hello"));
            var handler = new InvalidateCacheCommandHandler(memoryCacheService);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsType<SuccessResult>(result);
        }
    }
}