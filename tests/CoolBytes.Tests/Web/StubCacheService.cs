using CoolBytes.WebAPI.Services.Caching;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolBytes.Tests.Web
{
    public class StubCacheService : ICacheService
    {
        public ValueTask<T> GetOrAddAsync<T>(Expression<Func<Task<T>>> factoryExpression)
        {
            var result = factoryExpression.Compile()();

            return new ValueTask<T>(result);
        }

        public ValueTask<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string key, Func<Task<T>> factory)
        {
            throw new NotImplementedException();
        }
    }
}
