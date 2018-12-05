using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services.Caching
{
    public interface ICacheService
    {
        /// <summary>
        /// Gets T from the cache. If it doesn't exist than it uses the factory to generate the key and value.
        /// </summary>
        /// <typeparam name="T">The T value.</typeparam>
        /// <param name="factoryExpression">The factory to generate the key and value.</param>
        /// <param name="arguments">Optional arguments to prevent key collisions.</param>
        /// <returns>The value.</returns>
        ValueTask<T> GetOrAddAsync<T>(Expression<Func<Task<T>>> factoryExpression, params object[] arguments);

        /// <summary>
        /// Gets T from the cache.
        /// </summary>
        /// <typeparam name="T">The T value.</typeparam>
        /// <param name="key">The key associated with the value.</param>
        /// <returns>The value.</returns>
        ValueTask<T> GetAsync<T>(string key);

        /// <summary>
        /// Sets T in the cache.
        /// </summary>
        /// <typeparam name="T">The T value.</typeparam>
        /// <param name="key">The key associated with the value.</param>
        /// <param name="factory">The factory to produce the value.</param>
        /// <returns>A task.</returns>
        ValueTask AddAsync<T>(string key, Func<Task<T>> factory);

        /// <summary>
        /// Removes all items from the cache.
        /// </summary>
        /// <returns></returns>
        ValueTask RemoveAllAsync();
    }
}
