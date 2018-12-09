using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class BaseHandler
    {
        private readonly IServiceProvider _serviceProvider;
        protected AppDbContext Context { get; }
        protected ICacheService CacheService { get; }

        public BaseHandler(AppDbContext context, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Context = context;
            CacheService = serviceProvider.GetService<ICacheService>();
        }

        public async ValueTask<T> GetOrAddAsync<T>(Expression<Func<Task<T>>> factoryExpression, params object[] arguments)
        {
            if (await CacheEnabledAsync())
                return await CacheService.GetOrAddAsync(factoryExpression);

            var result = await factoryExpression.Compile()();

            return result;
        }

        private async Task<bool> CacheEnabledAsync()
        {
            var httpContext = _serviceProvider.GetService<IHttpContextAccessor>().HttpContext;

            if (!httpContext.Request.Headers.ContainsKey("X-CACHE-ENABLED"))
                return true;

            var userResult = await TryGetCurrentUserAsync();

            return !userResult.IsSuccess;
        }

        private async Task<Result<User>> TryGetCurrentUserAsync()
        {
            var userService = _serviceProvider.GetService<IUserService>();
            var result = await userService.TryGetCurrentUserAsync();

            return result;
        }
    }
}