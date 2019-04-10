using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Utils;
using CoolBytes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.Services
{
    [Scoped]
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetOrCreateCurrentUserAsync()
        {
            var identifier = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException(nameof(identifier));

            var user = await _appDbContext.Users.SingleOrDefaultAsync(u => u.Identifier == identifier);
            if (user != null)
                return user;

            user = new User(identifier);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<Result<User>> TryGetCurrentUserAsync()
        {
            var identifier = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identifier))
                return Result<User>.NotFoundResult();

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Identifier == identifier);

            return user != null ? user.ToSuccessResult() : Result<User>.NotFoundResult();
        }
    }
}