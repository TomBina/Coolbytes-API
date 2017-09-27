using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Services;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsTests : IClassFixture<Fixture>, IAsyncLifetime
    {
        private readonly AppDbContext _appDbContext;
        private IUserService _userService;
        private readonly Fixture _fixture;

        public AuthorsTests(Fixture fixture)
        {
            _fixture = fixture;
            _appDbContext = fixture.GetNewContext();
        }

        [Fact]
        public async Task GetAuthorQueryHandler_ReturnsAuthor()
        {
            var author = await AddAuthor();
            var getAuthorQueryHandler = new GetAuthorQueryHandler(_appDbContext, _userService);
            var message = new GetAuthorQuery();

            var result = await getAuthorQueryHandler.Handle(message);

            Assert.Equal(author.AuthorProfile.FirstName, result.FirstName);
        }

        private async Task<Author> AddAuthor()
        {
            var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
            var authorValidator = new AuthorValidator(_appDbContext);
            var user = await _userService.GetUser();

            var author = await Author.Create(user, authorProfile, authorValidator);

            using (var context = _fixture.GetNewContext())
            {
                context.Add(author);
                await context.SaveChangesAsync();
            }

            return author;
        }

        [Fact]
        public async Task AddAuthorCommandHandler_ReturnsAuthor()
        {
            var authorValidator = new AuthorValidator(_appDbContext);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(_appDbContext, _userService, authorValidator);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result = await addAuthorCommandHandler.Handle(message);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddAuthorCommandHandler_AddingSecondTime_ThrowsException()
        {
            var authorValidator = new AuthorValidator(_appDbContext);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(_appDbContext, _userService, authorValidator);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result1 = await addAuthorCommandHandler.Handle(message);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await addAuthorCommandHandler.Handle(message);
            });
        }

        public async Task InitializeAsync()
        {
            var user = new User("Test");

            using (var context = _fixture.GetNewContext())
            {
                _appDbContext.Add(user);
                await context.SaveChangesAsync();
            }

            var userService = new Mock<IUserService>();
            userService.Setup(exp => exp.GetUser()).ReturnsAsync(user);
            _userService = userService.Object;
        }

        public async Task DisposeAsync()
        {
            _appDbContext.Users.RemoveRange(_appDbContext.Users.ToArray());
            _appDbContext.Authors.RemoveRange(_appDbContext.Authors.ToArray());

            await _appDbContext.SaveChangesAsync();
            _appDbContext.Dispose();
        }
    }
}
