using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services.Caching
{
    public class CacheKeyGenerator
    {
        public string GetKey<T>(Expression<Func<Task<T>>> factoryExpression)
        {
            var memberExpression = factoryExpression.Body as MethodCallExpression;

            if (memberExpression == null)
                throw new InvalidOperationException("Expression not valid!");

            var methodInfo = memberExpression.Method;

            var key = methodInfo.DeclaringType.FullName + "_" + methodInfo.Name;

            return key;
        }
    }
}