using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services.Caching
{
    public interface ICacheService
    {
        ValueTask<T> GetOrAddAsync<T>(Expression<Func<Task<T>>> factoryExpression);

        ValueTask<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, Func<Task<T>> factory);
    }
}
