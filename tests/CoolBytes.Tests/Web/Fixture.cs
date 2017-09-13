using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.AutoMapper;
using CoolBytes.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CoolBytes.Tests.Web
{
    public class Fixture : IDisposable
    {
        private static readonly Random Random = new Random();

        public Fixture()
        {
            var dbName = "Test" + Random.Next();
            Context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(dbName).Options);

            Mapper.Initialize(config => config.AddProfile(new DefaultProfile()));

            var userService = new Mock<IUserService>();
            var user = new User("test");

            userService.Setup(exp => exp.GetUser()).ReturnsAsync(user);
            UserService = userService.Object;
        }

        public AppDbContext Context { get; }
        public IUserService UserService { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
