using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsTests : TestBase, IClassFixture<TestContext>, IAsyncLifetime
    {
        public AuthorsTests(TestContext testContext) : base(testContext)
        {
            InitUserService();
            InitAuthorService();
        }

        public async Task InitializeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task GetAuthorQueryHandler_ReturnsAuthor()
        {
            await AddAuthor();
            var getAuthorQueryHandler = new GetAuthorQueryHandler(AuthorService);
            var message = new GetAuthorQuery() { IncludeProfile = true };

            var result = await getAuthorQueryHandler.Handle(message, CancellationToken.None);

            Assert.Equal("Tom", result.FirstName);
        }

        private async Task AddAuthor()
        {
            using (var context = TestContext.CreateNewContext())
            {
                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(context);
                var user = await UserService.GetUser();

                var author = await Author.Create(user, authorProfile, authorValidator);
                context.Authors.Add(author);
                await context.SaveChangesAsync();
            }
        }

        private async Task<Image> AddImage()
        {
            using (var context = TestContext.CreateNewContext())
            {
                var imageFactory = CreateImageFactory();
                var file = CreateFileMock().Object;
                var image = await imageFactory.Create(file.OpenReadStream(), file.FileName, file.ContentType);

                context.Images.Add(image);
                await context.SaveChangesAsync();

                return image;
            }
        }

        [Fact]
        public async Task AddAuthorCommandHandler_WithExperiences_ReturnsAuthor()
        {
            var image = await AddImage();
            var experiences = new List<ExperienceDto>();
            var experienceDto = new ExperienceDto()
            {
                Color = "#000000",
                Name = "Testfile",
                ImageId = image.Id
            };
            experiences.Add(experienceDto);

            var authorValidator = new AuthorValidator(Context);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(Context, UserService, authorValidator);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me", Experiences = experiences };

            var result = await addAuthorCommandHandler.Handle(message, CancellationToken.None);

            Assert.Equal("Testfile", result.Experiences.First().Name = experienceDto.Name);
        }

        [Fact]
        public async Task AddAuthorCommandHandler_AddingSecondTime_ThrowsException()
        {
            await AddAuthor();

            var authorValidator = new AuthorValidator(Context);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(Context, UserService, authorValidator);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await addAuthorCommandHandler.Handle(message, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateAuthorCommandHandler_WithExperiences_UpdatesAuthor()
        {
            await AddAuthor();
            var image = await AddImage();
            var experiences = new List<ExperienceDto>();
            var experienceDto = new ExperienceDto()
            {
                Color = "#000000",
                Name = "Testfile",
                ImageId = image.Id
            };
            experiences.Add(experienceDto);

            var message = new UpdateAuthorCommand() { FirstName = "Test", LastName = "Test", About = "Test", Experiences = experiences };
            var handler = new UpdateAuthorCommandHandler(Context, AuthorService);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal("Testfile", result.Experiences.First().Name = experienceDto.Name);
        }

        [Fact]
        public async Task UpdateAuthorCommandHandler_WithImage_ReturnsAuthor()
        {
            await AddAuthor();
            var image = await AddImage();
            var handler = new UpdateAuthorCommandHandler(Context, AuthorService);

            var message = new UpdateAuthorCommand()
            {
                FirstName = "Tom",
                LastName = "Bina",
                About = "About me",
                ImageId = image.Id
            };

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result.Image);
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
