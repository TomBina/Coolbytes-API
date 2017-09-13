using System;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class AddAuthorCommandHandler : IAsyncRequestHandler<AddAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;

        public AddAuthorCommandHandler(AppDbContext appDbContext, IUserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
        }

        public async Task<AuthorViewModel> Handle(AddAuthorCommand message)
        {
            var user = await _userService.GetUser();

            if (await _appDbContext.Authors.AnyAsync(a => a.UserId == user.UserId))
                throw new InvalidOperationException("Only one author is allowed per user");

            var author = new Author(user, message.FirstName, message.LastName, message.About);
            _appDbContext.Authors.Add(author);
            await _appDbContext.SaveChangesAsync();

            return Mapper.Map<AuthorViewModel>(author);
        }
    }
}