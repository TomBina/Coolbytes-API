using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using MediatR;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class AddAuthorCommandHandler : IAsyncRequestHandler<AddAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly IAuthorValidator _authorValidator;

        public AddAuthorCommandHandler(AppDbContext appDbContext, IUserService userService, IAuthorValidator authorValidator)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            _authorValidator = authorValidator;
        }

        public async Task<AuthorViewModel> Handle(AddAuthorCommand message)
        {
            var user = await _userService.GetUser();
            var authorProfile = new AuthorProfile(message.FirstName, message.LastName, message.About);
            var author = await Author.Create(user, authorProfile, _authorValidator);

            _appDbContext.Authors.Add(author);
            await _appDbContext.SaveChangesAsync();

            return Mapper.Map<AuthorViewModel>(author);
        }
    }
}