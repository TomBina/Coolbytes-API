﻿using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Services.Caching
{
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

            var result = await _userService.TryGetCurrentUserAsync();

            return !result.IsSuccess;
        }
    }
}