using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CoolBytes.Services
{
    [Inject(typeof(IAuthorService))]
    [Inject(typeof(IAuthorSearchService))]
    public class AuthorService : IAuthorService, IAuthorSearchService
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
            var user = await _userService.TryGetCurrentUserAsync();

            if (!user)
                return null;

            return await _context.Authors.FirstOrDefaultAsync(a => a.UserId == user.Payload.Id);
        }

        public async Task<Author> GetAuthorWithProfile()
        {
            var user = await _userService.TryGetCurrentUserAsync();

            if (!user)
                return null;

            return await QueryAuthorWithProfile().FirstOrDefaultAsync(a => a.UserId == user.Payload.Id);
        }

        public async Task<Author> GetAuthorWithProfile(int id)
            => await QueryAuthorWithProfile().FirstOrDefaultAsync(a => a.Id == id);

        private IIncludableQueryable<Author, Image> QueryAuthorWithProfile()
            => _context.Authors.Include(a => a.AuthorProfile)
                               .ThenInclude(ap => ap.Image)
                               .Include(a => a.AuthorProfile.SocialHandles)
                               .Include(a => a.AuthorProfile.Experiences)
                               .ThenInclude(e => e.Image);
    }
}
