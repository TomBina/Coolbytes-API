﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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

            var userService = new Mock<IUserService>();
            var user = new User("test");
            userService.Setup(exp => exp.GetUser()).ReturnsAsync(user);
            serviceCollection.AddSingleton<IUserService>(userService.Object);

            serviceCollection.AddMediatR(typeof(Startup));
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task AddAuthor_ReturnsAuthor()
        {
            var controller = new AuthorsController(_serviceProvider.GetService<IMediator>());
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            var result = await controller.AddAuthor(message);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddAuthor_AddingSecondTime_ReturnsBadRequest()
        {
            var controller = new AuthorsController(_serviceProvider.GetService<IMediator>());
            var message = new AddAuthorCommand() { FirstName = "Tom", LastName = "Bina", About = "About me" };

            await controller.AddAuthor(message);
            var result = await controller.AddAuthor(message);

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
