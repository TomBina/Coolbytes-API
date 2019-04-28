using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.Services;
using CoolBytes.Services.Caching;
using CoolBytes.WebAPI;
using CoolBytes.WebAPI.Features.Authors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Authors.CQ;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Authors
{
    public class AuthorsControllerTests : IClassFixture<TestContext>
    {
        private readonly IServiceProvider _serviceProviderBuilder;

        public AuthorsControllerTests()
        {            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContextPool<AppDbContext>(builder => builder.UseInMemoryDatabase("Test"));
            serviceCollection.AddScoped<IAuthorValidator, AuthorValidator>();
            serviceCollection.AddScoped<ImageFactory>(sp => null);
            serviceCollection.AddScoped<IConfiguration>(sp => null);
            serviceCollection.AddScoped<ICacheService>(sp => new StubCacheService());

            var userService = new Mock<IUserService>();
            var user = new User("test");
            userService.Setup(exp => exp.GetOrCreateCurrentUserAsync()).ReturnsAsync(user);
            userService.Setup(exp => exp.TryGetCurrentUserAsync()).ReturnsAsync(user.ToSuccessResult());
            serviceCollection.AddSingleton(userService.Object);

            serviceCollection.AddMediatR(typeof(Startup));
            _serviceProviderBuilder = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task AddAuthor_ReturnsAuthor()
        {
            var controller = new AuthorsController(_serviceProviderBuilder.GetService<IMediator>());
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result = await controller.Add(message);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddAuthor_AddingSecondTime_ReturnsBadRequest()
        {
            var controller = new AuthorsController(_serviceProviderBuilder.GetService<IMediator>());
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            await controller.Add(message);
            var result = await controller.Add(message);

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
