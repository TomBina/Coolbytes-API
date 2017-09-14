using System.Threading.Tasks;
using AutoMapper;
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
            var user = await _userService.GetUser();
            var author =  await _appDbContext.Authors.SingleOrDefaultAsync(a => a.UserId == user.Id);

            return Mapper.Map<AuthorViewModel>(author);
        }
    }
}