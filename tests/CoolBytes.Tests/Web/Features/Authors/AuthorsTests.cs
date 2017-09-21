using System;
using System.Linq;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Services;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsTests : IClassFixture<Fixture>, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly Fixture _fixture;

        public AuthorsTests(Fixture fixture)
        {
            _fixture = fixture;
            _appDbContext = fixture.GetNewContext();
            _userService = fixture.UserService;
        }

        [Fact]
        public async Task GetAuthorQueryHandler_ReturnsAuthor()
        {
            var user = await _userService.GetUser();
            var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
            var authorData = new AuthorData(_appDbContext);
            var author = await Author.Create(user, authorProfile, authorData);

            using (var context = _fixture.GetNewContext())
            {
                context.Authors.Add(author);
                await context.SaveChangesAsync();
            }

            var getAuthorQueryHandler = new GetAuthorQueryHandler(_appDbContext, _userService);
            var message = new GetAuthorQuery();

            var result = await getAuthorQueryHandler.Handle(message);

            Assert.Equal(author.AuthorProfile.FirstName, result.FirstName);
        }

        [Fact]
        public async Task AddAuthorCommandHandler_ReturnsAuthor()
        {
            var addAuthorCommandHandler = new AddAuthorCommandHandler(_appDbContext, _userService);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result = await addAuthorCommandHandler.Handle(message);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddAuthorCommandHandler_AddingSecondTime_ThrowsException()
        {
            var addAuthorCommandHandler = new AddAuthorCommandHandler(_appDbContext, _userService);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result1 = await addAuthorCommandHandler.Handle(message);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await addAuthorCommandHandler.Handle(message);
            });
        }

        public void Dispose()
        {
            _appDbContext.Authors.RemoveRange(_appDbContext.Authors.ToArray());
            _appDbContext.Users.RemoveRange(_appDbContext.Users.ToArray());

            _appDbContext.SaveChanges();
            _appDbContext.Dispose();
        }
    }
}
