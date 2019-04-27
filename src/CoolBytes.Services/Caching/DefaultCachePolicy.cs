using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Attributes;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.Services.Caching
{
    [Inject(typeof(ICachePolicy))]
    public class DefaultCachePolicy : ICachePolicy
    {
        private readonly HttpContext _httpContext;
        private readonly IUserService _userService;

        public DefaultCachePolicy(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _userService = userService;
        }

        public async Task<bool> IsCacheActiveAsync()
        {
            if (!_httpContext.Request.Headers.ContainsKey("X-CACHE-ENABLED"))
                return true;

            bool.TryParse(_httpContext.Request.Headers["X-CACHE-ENABLED"], out var cacheEnabled);

            if (cacheEnabled)
                return true;

            var result = await _userService.TryGetCurrentUserAsync();

            return !result.IsSuccess;
        }
    }
}