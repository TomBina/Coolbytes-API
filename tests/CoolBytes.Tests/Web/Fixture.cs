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
using Microsoft.Extensions.Configuration;
using Moq;

namespace CoolBytes.Tests.Web
{
    public class Fixture
    {
        private static readonly Random Random = new Random();

        public Fixture()
        {
            Mapper.Initialize(config => config.AddProfile(new DefaultProfile()));

            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("Test" + Random.Next()).Options;

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["PhotosUri:Scheme"]).Returns("testdata");
            configuration.Setup(c => c["PhotosUri:Host"]).Returns("testserver.com");
            configuration.Setup(c => c["PhotosUri:Port"]).Returns("80");
            Configuration = configuration.Object;
        }

        public IConfiguration Configuration { get; }

        public AppDbContext GetNewContext() => new AppDbContext(_options);
        private readonly DbContextOptions<AppDbContext> _options;
    }
}
