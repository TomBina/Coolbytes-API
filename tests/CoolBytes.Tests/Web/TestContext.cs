using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.AutoMapper;
using CoolBytes.WebAPI.Services.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;

namespace CoolBytes.Tests.Web
{
    public class TestContext : IDisposable
    {
        private static readonly Random Random = new Random();
        public string TempDirectory { get; } = Path.Combine(Environment.CurrentDirectory, $"dir{Random.Next()}");
        private DbContextOptions<AppDbContext> _options;

        private static bool _initialized;
        private static readonly object Mutex = new object();

        public IConfiguration Configuration { get; private set; }

        public TestContext()
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
            lock (Mutex)
            {
                if (_initialized)
                    return;

                Mapper.Initialize(config => config.AddProfile(new DefaultProfile()));
                _initialized = true;

            }
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

        public ICacheService CacheService() => new StubCacheService();

        public void Dispose()
        {
            if (Directory.Exists(TempDirectory))
                Directory.Delete(TempDirectory, recursive: true);
        }
    }
}
