using CoolBytes.WebAPI.Services.Caching;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolBytes.Tests.Web
{
    public class StubCacheService : ICacheService
    {
        public ValueTask<T> GetOrAddAsync<T>(Expression<Func<Task<T>>> factoryExpression, params object[] arguments)
        {
            var result = factoryExpression.Compile()();

            return new ValueTask<T>(result);
        }

        public ValueTask<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask AddAsync<T>(string key, Func<Task<T>> factory)
        {
            throw new NotImplementedException();
        }

        public ValueTask RemoveAllAsync()
        {
            return new ValueTask();
        }
    }
}
