using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Utils;

namespace CoolBytes.WebAPI.Services.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, object> _store = new ConcurrentDictionary<string, object>();
        private readonly ICachePolicy _cachePolicy;
        private readonly CacheKeyGenerator _cacheKeyGenerator;

        public MemoryCacheService(ICachePolicy cachePolicy, CacheKeyGenerator cacheKeyGenerator)
        {
            _cachePolicy = cachePolicy;
            _cacheKeyGenerator = cacheKeyGenerator;
        }

        public async Task<T> GetOrAddAsync<T>(Expression<Func<Task<T>>> factoryExpression, params object[] arguments)
        {
            var cacheActive = await _cachePolicy.IsCacheActiveAsync();

            if (!cacheActive)
                return await factoryExpression.Compile()();

            var key = GenerateKey(factoryExpression, arguments);
            var value = await GetAsync<T>(key);

            if (value != null)
                return value;

            await AddAsync(key, factoryExpression.Compile());
            value = await GetAsync<T>(key);

            return value;
        }

        private string GenerateKey<T>(Expression<Func<Task<T>>> factoryExpression, object[] arguments)
        {
            var key = _cacheKeyGenerator.GetKey(factoryExpression, arguments);
            return key;
        }

        public ValueTask<T> GetAsync<T>(string key)
        {
            var value = (T)_store.Get(key);

            return new ValueTask<T>(value);
        }

        public async ValueTask AddAsync<T>(string key, Func<Task<T>> factory)
        {
            if (_store.ContainsKey(key))
                return;

            var entry = await factory();
            _store.TryAdd(key, entry);
        }

        public ValueTask RemoveAllAsync()
        {
            _store.Clear();

            return new ValueTask();
        }
    }
}