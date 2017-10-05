using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class GetAuthorQueryHandler : IAsyncRequestHandler<GetAuthorQuery, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;

        public GetAuthorQueryHandler(AppDbContext appDbContext, IUserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
        }

        public async Task<AuthorViewModel> Handle(GetAuthorQuery message)
        {
            var author = await GetAuthor();

            return CreateViewModel(author);
        }

        private async Task<Author> GetAuthor()
        {
            var user = await _userService.GetUser();
            var author = await _appDbContext.Authors.AsNoTracking()
                .Include(a => a.AuthorProfile)
                .Include(a => a.AuthorProfile.Image)
                .SingleOrDefaultAsync(a => a.UserId == user.Id);

            return author;
        }

        private AuthorViewModel CreateViewModel(Author author) => Mapper.Map<AuthorViewModel>(author);
    }
}