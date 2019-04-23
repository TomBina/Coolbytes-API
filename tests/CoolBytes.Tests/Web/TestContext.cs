using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using CoolBytes.Services.Caching;
using CoolBytes.Services.ImageFactories;

namespace CoolBytes.Tests.Web
{
    /// <summary>
    /// Provides all contextual information on a per-test-class basis.
    /// </summary>
    public class TestContext : IDisposable
    {
        public IConfiguration Configuration { get; private set; }

        private static readonly Random Random = new Random();
        private readonly string _tempDirectory = Path.Combine(Environment.CurrentDirectory, $"dir{Random.Next()}");
        private DbContextOptions<AppDbContext> _options;

        private static bool _initialized;
        private static readonly object Mutex = new object();

        public ServiceProviderBuilder ServiceProviderBuilder
            => new ServiceProviderBuilder();

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
            configuration.Setup(c => c["ImagesUploadPath"]).Returns(_tempDirectory);
            Configuration = configuration.Object;
        }

        public IHttpContextAccessor CreateHttpContextAccessor(Action<HttpContext> configure = null)
        {
            var accessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            accessor.Setup(h => h.HttpContext).Returns(httpContext);

            configure?.Invoke(httpContext);

            return accessor.Object;
        }

        public AppDbContext CreateNewContext()
            => new AppDbContext(_options);

        /// <summary>
        /// Create a fake cache.
        /// </summary>
        /// <returns>StubCacheService</returns>
        public StubCacheService CreateStubCacheService()
            => new StubCacheService();

        /// <summary>
        /// Creates a real memory cache.
        /// </summary>
        /// <param name="cachePolicy">When no policy is provided an empty only will be used.</param>
        /// <returns>MemoryCacheService</returns>
        public MemoryCacheService CreateMemoryCacheService(ICachePolicy cachePolicy = null)
        {
            if (cachePolicy == null)
                cachePolicy = new StubCachePolicy();

            var cacheKeyGenerator = new CacheKeyGenerator();

            return new MemoryCacheService(cachePolicy, cacheKeyGenerator);
        }

        public LocalImageFactory CreateImageFactory()
        {
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["ImagesUploadPath"]).Returns(_tempDirectory);

            var options = new LocalImageFactoryOptions(config.Object);
            var validator = new ImageFactoryValidator();
            var imageFactory = new LocalImageFactory(options, validator);
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
            if (Directory.Exists(_tempDirectory))
                Directory.Delete(_tempDirectory, recursive: true);
        }
    }
}