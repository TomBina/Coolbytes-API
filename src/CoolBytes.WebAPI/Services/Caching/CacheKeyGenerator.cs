using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services.Caching
{
    public class CacheKeyGenerator
    {
        public string GetKey<T>(Expression<Func<Task<T>>> factoryExpression, params object[] arguments)
        {
            if (!(factoryExpression.Body is MethodCallExpression memberExpression))
                throw new InvalidOperationException("Expression not valid!");

            var methodInfo = memberExpression.Method;
            var key = methodInfo.DeclaringType.FullName + "_" + methodInfo.Name;

            if (arguments.Length > 0)
                key += "_" + string.Join("_", arguments);

            return key;
        }
    }
}