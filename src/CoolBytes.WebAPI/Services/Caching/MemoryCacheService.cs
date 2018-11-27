using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Utils;

namespace CoolBytes.WebAPI.Services.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private static readonly ConcurrentDictionary<string, object> Store = new ConcurrentDictionary<string, object>();

        public async ValueTask<T> GetOrAddAsync<T>(Expression<Func<Task<T>>> factoryExpression)
        {
            var key = GetKey(factoryExpression);

            var value = await GetAsync<T>(key);

            if (value != null)
                return value;

            await SetAsync(key, factoryExpression.Compile());
            value = await GetAsync<T>(key);

            return value;
        }

        private string GetKey<T>(Expression<Func<Task<T>>> factoryExpression)
        {
            var memberExpression = factoryExpression.Body as MethodCallExpression;

            if (memberExpression == null)
                throw new InvalidOperationException("Expression not valid!");

            return memberExpression.Method.Name;
        }

        public ValueTask<T> GetAsync<T>(string key)
        {
            var value = (T)Store.Get(key);

            return new ValueTask<T>(value);
        }

        public async Task SetAsync<T>(string key, Func<Task<T>> factory)
        {
            if (Store.ContainsKey(key))
                return;

            var entry = await factory();
            Store.TryAdd(key, entry);
        }
    }
}