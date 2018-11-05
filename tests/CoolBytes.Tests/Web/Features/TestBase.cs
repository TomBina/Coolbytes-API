using CoolBytes.Core.Factories;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;

namespace CoolBytes.Tests.Web.Features
{
    public abstract class TestBase
    {
        protected AppDbContext Context;
        protected Fixture Fixture;
        protected IUserService UserService;
        protected AuthorService AuthorService;

        protected TestBase(Fixture fixture)
        {
            Fixture = fixture;
            Context = fixture.CreateNewContext();
        }

        protected void InitUserService(User user)
        {
            var userService = new Mock<IUserService>();
            userService.Setup(exp => exp.GetUser()).ReturnsAsync(user);
            UserService = userService.Object;
        }

        protected void InitUserService()
        {
            var user = new User("Test");

            InitUserService(user);
        }

        protected void InitAuthorService()
        {
            AuthorService = new AuthorService(UserService, Context);
        }

        protected ImageFactory CreateImageFactory()
        {
            var options = new ImageFactoryOptions(Fixture.TempDirectory);
            var validator = new ImageFactoryValidator();
            var imageFactory = new ImageFactory(options, validator);
            return imageFactory;
        }

        protected Mock<IFormFile> CreateFileMock()
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(e => e.FileName).Returns("testimage.png");
            mock.Setup(e => e.ContentType).Returns("image/png");
            mock.Setup(e => e.OpenReadStream()).Returns(() => File.OpenRead("assets/testimage.png"));
            return mock;
        }
    }
}