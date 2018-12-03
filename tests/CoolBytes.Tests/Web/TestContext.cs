using AutoMapper;
using CoolBytes.Core.Factories;
using CoolBytes.Data;
using CoolBytes.WebAPI;
using CoolBytes.WebAPI.Services.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;

namespace CoolBytes.Tests.Web
{
    /// <summary>
    /// Provides all contextual information on a per-test-class basis.
    /// </summary>
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

        private static void InitAutoMapper()
        {
            lock (Mutex)
            {
                if (_initialized)
                    return;

                Mapper.Initialize(c => c.AddProfiles(typeof(Program).Assembly));
                _initialized = true;

            }
        }

        private void InitDb()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("DB" + Random.Next()).Options;
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

        public AppDbContext CreateNewContext() 
            => new AppDbContext(_options);

        public StubCacheService CreateStubCacheService 
            => new StubCacheService();

        public MemoryCacheService CreateMemoryCacheService 
            => new MemoryCacheService(new CacheKeyGenerator());

        public ImageFactory CreateImageFactory()
        {
            var options = new ImageFactoryOptions(TempDirectory);
            var validator = new ImageFactoryValidator();
            var imageFactory = new ImageFactory(options, validator);
            return imageFactory;
        }

        public Mock<IFormFile> CreateFileMock()
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(e => e.FileName).Returns("testimage.png");
            mock.Setup(e => e.ContentType).Returns("image/png");
            mock.Setup(e => e.OpenReadStream()).Returns(() => File.OpenRead("assets/testimage.png"));
            return mock;
        }

        public void Dispose()
        {
            if (Directory.Exists(TempDirectory))
                Directory.Delete(TempDirectory, recursive: true);
        }
    }
}
