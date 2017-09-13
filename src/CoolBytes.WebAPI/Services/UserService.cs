using CoolBytes.Core.Extensions;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> GetUser()
        {
            var identifier = ClaimsPrincipal.Current.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            identifier.IsNotNullOrWhiteSpace();

            var user = await _appDbContext.Users.SingleOrDefaultAsync(u => u.Identifier == identifier) ?? new User(identifier);
            return user;
        }
    }
}