using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
using CoolBytes.Services;
using CoolBytes.WebAPI.Features.Authors.CQ;
using CoolBytes.WebAPI.Features.Authors.DTO;
using CoolBytes.WebAPI.Features.Authors.Handlers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.WebAPI.Features.Authors.Profiles;
using CoolBytes.WebAPI.Features.Authors.ViewModels;
using CoolBytes.WebAPI.Features.Images.Profiles;
using CoolBytes.WebAPI.Features.Images.Profiles.Resolvers;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsTests : TestBase<TestContext>
    {
        private readonly IUserService _userService;
        private readonly AuthorService _authorService;

        public AuthorsTests(TestContext testContext) : base(testContext)
        {
            var user = new User("Test");
            var userService = new Mock<IUserService>();
            userService.Setup(exp => exp.GetOrCreateCurrentUserAsync()).ReturnsAsync(user);
            userService.Setup(exp => exp.TryGetCurrentUserAsync()).ReturnsAsync(user.ToSuccessResult());
            _userService = userService.Object;
            _authorService = new AuthorService(_userService, TestContext.CreateNewContext());
        }

        [Fact]
        public async Task GetAuthorQueryHandler_ReturnsAuthor()
        {
            await AddAuthor();
            var getAuthorQueryHandler = new GetAuthorQueryHandler(TestContext.CreateHandlerContext<AuthorViewModel>(RequestDbContext), _authorService);
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
                var user = await _userService.GetOrCreateCurrentUserAsync();

                var author = await Author.Create(user, authorProfile, authorValidator);
                context.Authors.Add(author);
                await context.SaveChangesAsync();
            }
        }

        private async Task<Image> AddImage()
        {
            using (var context = TestContext.CreateNewContext())
            {
                var imageService = TestContext.CreateImageService();
                var file = TestContext.CreateFileMock().Object;
                var image = await imageService.Save(file.OpenReadStream(), file.FileName, file.ContentType);

                context.Images.Add(image);
                await context.SaveChangesAsync();

                return image;
            }
        }

        private IMapper CreateMapper()
        {
            var sp = TestContext.ServiceProviderBuilder.Add(s =>
                s.AddTransient<IImageViewModelUrlResolver, LocalImageViewModelUrlResolver>()
                    .AddTransient<ImageViewModelResolver>()).Build();
            var profiles = new Profile[] { new ImageViewModelProfile(), new AuthorViewModelProfile() };
            var mapper = TestContext.CreateMapper(profiles, sp);
            return mapper;
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

            var authorValidator = new AuthorValidator(RequestDbContext);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(TestContext.CreateHandlerContext<AuthorViewModel>(RequestDbContext, CreateMapper()), _userService, authorValidator);
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me", Experiences = experiences };

            var result = await addAuthorCommandHandler.Handle(message, CancellationToken.None);

            Assert.Equal("Testfile", result.Experiences.First().Name = experienceDto.Name);
        }

        [Fact]
        public async Task AddAuthorCommandHandler_AddingSecondTime_ThrowsException()
        {
            await AddAuthor();

            var authorValidator = new AuthorValidator(RequestDbContext);
            var addAuthorCommandHandler = new AddAuthorCommandHandler(TestContext.CreateHandlerContext<AuthorViewModel>(RequestDbContext, CreateMapper()), _userService, authorValidator);
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
            var handler = new UpdateAuthorCommandHandler(TestContext.CreateHandlerContext<AuthorViewModel>(RequestDbContext, CreateMapper()), _authorService);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal("Testfile", result.Experiences.First().Name = experienceDto.Name);
        }

        [Fact]
        public async Task UpdateAuthorCommandHandler_WithImage_ReturnsAuthor()
        {
            await AddAuthor();
            var image = await AddImage();
            var handler = new UpdateAuthorCommandHandler(TestContext.CreateHandlerContext<AuthorViewModel>(RequestDbContext, CreateMapper()), _authorService);

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

        public override async Task DisposeAsync()
        {
            RequestDbContext.Users.RemoveRange(RequestDbContext.Users.ToArray());
            RequestDbContext.Authors.RemoveRange(RequestDbContext.Authors.ToArray());

            await RequestDbContext.SaveChangesAsync();
            RequestDbContext.Dispose();
        }
    }
}
