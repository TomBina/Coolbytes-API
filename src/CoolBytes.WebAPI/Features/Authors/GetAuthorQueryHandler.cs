using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class GetAuthorQueryHandler : IAsyncRequestHandler<GetAuthorQuery, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public GetAuthorQueryHandler(AppDbContext appDbContext, IUserService userService, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            _configuration = configuration;
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
                .Include(a => a.AuthorProfile.Photo)
                .SingleOrDefaultAsync(a => a.UserId == user.Id);

            return author;
        }

        private AuthorViewModel CreateViewModel(Author author)
        {
            var viewModel = Mapper.Map<AuthorViewModel>(author);
            viewModel?.Photo?.FormatPhotoUri(_configuration);

            return viewModel;
        }
    }
}