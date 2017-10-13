using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public AuthorService(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<Author> GetAuthor()
        {
            var user = await _userService.GetUser();
            return await _context.Authors.FirstOrDefaultAsync(a => a.UserId == user.Id);
        }

        public async Task<Author> GetAuthorWithProfile()
        {
            var user = await _userService.GetUser();
            return await _context.Authors.Include(a => a.AuthorProfile).ThenInclude(ap => ap.Image).FirstOrDefaultAsync(a => a.UserId == user.Id);
        }
    }
}
