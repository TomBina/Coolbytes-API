using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Factories;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.AutoMapper;
using CoolBytes.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web
{
    public class Fixture : IDisposable
    {
        private static readonly Random Random = new Random();
        public string TempDirectory { get; } = Path.Combine(Environment.CurrentDirectory, $"dir{Random.Next()}");
        private DbContextOptions<AppDbContext> _options;

        public IConfiguration Configuration { get; private set; }

        public Fixture()
        {
            InitAutoMapper();
            InitDb();
            InitConfiguration();
        }

        private void InitDb()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("DB" + Random.Next()).Options;
        }

        private static void InitAutoMapper()
        {
            Mapper.Initialize(config => config.AddProfile(new DefaultProfile()));
        }

        private void InitConfiguration()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["ImagesUri:Scheme"]).Returns("testdata");
            configuration.Setup(c => c["ImagesUri:Host"]).Returns("testserver.com");
            configuration.Setup(c => c["ImagesUri:Port"]).Returns("80");
            configuration.Setup(c => c["ImagesUploadPath"]).Returns(TempDirectory);
            Configuration = configuration.Object;
        }

        public AppDbContext CreateNewContext() => new AppDbContext(_options);

        public void Dispose()
        {
            if (Directory.Exists(TempDirectory))
                Directory.Delete(TempDirectory, recursive: true);
        }
    }
}
