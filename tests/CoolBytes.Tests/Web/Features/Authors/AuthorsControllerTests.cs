using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsControllerTests : IClassFixture<Fixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public AuthorsControllerTests()
        {            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContextPool<AppDbContext>(builder => builder.UseInMemoryDatabase("Test"));
            serviceCollection.AddScoped<IAuthorValidator, AuthorValidator>();
            serviceCollection.AddScoped<IImageFactory>(sp => null);
            serviceCollection.AddScoped<IConfiguration>(sp => null);

            var userService = new Mock<IUserService>();
            var user = new User("test");
            userService.Setup(exp => exp.GetUser()).ReturnsAsync(user);
            serviceCollection.AddSingleton(userService.Object);

            serviceCollection.AddMediatR(typeof(Startup));
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task AddAuthor_ReturnsAuthor()
        {
            var controller = new AuthorsController(_serviceProvider.GetService<IMediator>());
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result = await controller.Add(message);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddAuthor_AddingSecondTime_ReturnsBadRequest()
        {
            var controller = new AuthorsController(_serviceProvider.GetService<IMediator>());
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            await controller.Add(message);
            var result = await controller.Add(message);

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
