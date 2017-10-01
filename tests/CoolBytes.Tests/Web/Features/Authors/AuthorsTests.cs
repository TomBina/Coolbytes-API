using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Services;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsTests : TestBase, IClassFixture<Fixture>, IAsyncLifetime
    {
        public AuthorsTests(Fixture fixture) : base(fixture)
        {
            InitUserService();
        }

        public async Task InitializeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task GetAuthorQueryHandler_ReturnsAuthor()
        {
            await AddAuthor();
            var getAuthorQueryHandler = new GetAuthorQueryHandler(Context, UserService, Fixture.Configuration);
            var message = new GetAuthorQuery();

            var result = await getAuthorQueryHandler.Handle(message);

            Assert.Equal("Tom", result.FirstName);
        }

        private async Task AddAuthor()
        {
           using (var context = Fixture.CreateNewContext())
            {
                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(context);
                var user = await UserService.GetUser();

                var author = await Author.Create(user, authorProfile, authorValidator);
                context.Authors.Add(author);
                await context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AddAuthorCommandHandler_ReturnsAuthor()
        {
            var authorValidator = new AuthorValidator(Context);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(Context, UserService, authorValidator, Fixture.Configuration, null);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result = await addAuthorCommandHandler.Handle(message);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddAuthorCommandHandler_WithPhoto_ReturnsAuthor()
        {
            var photoFactory = CreatePhotoFactory();
            var authorValidator = new AuthorValidator(Context);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(Context, UserService, authorValidator, Fixture.Configuration, photoFactory);
            var fileMock = CreateFileMock();
            var file = fileMock.Object;

            var message = new AddAuthorCommand()
            {
                FirstName = "Tom",
                LastName = "Bina",
                About = "About me",
                File = file
            };

            var result = await addAuthorCommandHandler.Handle(message);

            Assert.NotNull(result.Photo.PhotoUri);
        }

        [Fact]
        public async Task AddAuthorCommandHandler_AddingSecondTime_ThrowsException()
        {
            var authorValidator = new AuthorValidator(Context);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(Context, UserService, authorValidator, Fixture.Configuration, null);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            await addAuthorCommandHandler.Handle(message);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await addAuthorCommandHandler.Handle(message);
            });
        }

        [Fact]
        public async Task UpdateAuthorCommandHandler_UpdatesAuthor()
        {
            await AddAuthor();

            var message = new UpdateAuthorCommand() { FirstName = "Test", LastName = "Test", About = "Test" };
            var handler = new UpdateAuthorCommandHandler(Context, UserService, Fixture.Configuration, CreatePhotoFactory());

            var result = await handler.Handle(message);

            Assert.Equal("Test", result.FirstName);
        }

        [Fact]
        public async Task UpdateAuthorCommandHandler_WithPhoto_ReturnsAuthor()
        {
            await AddAuthor();

            var photoFactory = CreatePhotoFactory();
            var handler = new UpdateAuthorCommandHandler(Context, UserService, Fixture.Configuration, photoFactory);

            var file = CreateFileMock().Object;
            var message = new UpdateAuthorCommand()
            {
                FirstName = "Tom",
                LastName = "Bina",
                About = "About me",
                File = file
            };

            var result = await handler.Handle(message);

            Assert.NotNull(result.Photo);
        }

        public async Task DisposeAsync()
        {
            Context.Users.RemoveRange(Context.Users.ToArray());
            Context.Authors.RemoveRange(Context.Authors.ToArray());

            await Context.SaveChangesAsync();
            Context.Dispose();
        }
    }
}
