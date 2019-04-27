using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolBytes.Services.Caching
{
    public interface ICacheKeyGenerator
    {
        string GetKey<T>(Expression<Func<Task<T>>> factoryExpression, params object[] arguments);
    }
}