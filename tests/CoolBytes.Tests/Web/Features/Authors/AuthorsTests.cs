using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Services;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsTests : IClassFixture<Fixture>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;

        public AuthorsTests(Fixture fixture)
        {
            _appDbContext = fixture.Context;
            _userService = fixture.UserService;
        }

        [Fact]
        public async Task AddAuthorCommandHandler_ReturnsAuthor()
        {
            var addAuthorCommandHandler = new AddAuthorCommandHandler(_appDbContext, _userService);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me"};

            var result = await addAuthorCommandHandler.Handle(message);

            Assert.NotNull(result);
        }
    }
}
