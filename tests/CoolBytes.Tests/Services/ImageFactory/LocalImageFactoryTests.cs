using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Services.Images.Factories;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Services.ImageFactory
{
    public class LocalImageFactoryTests : IDisposable
    {
        private readonly string _uploadPath = Environment.CurrentDirectory + "/uploads";
        private readonly LocalImageFactoryOptions _localImageFactoryOptions;
        private readonly ImageFactoryValidator _imageFactoryValidator;

        public LocalImageFactoryTests()
        {
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["ImagesUploadPath"]).Returns(_uploadPath);

            _localImageFactoryOptions = new LocalImageFactoryOptions(config.Object);
            _imageFactoryValidator = new ImageFactoryValidator();
        }

        [Fact]
        public async Task LocalImageFactory_Valid_CreatesImage()
        {
            var imageFactory = new LocalImageFactory(_localImageFactoryOptions, _imageFactoryValidator);

            using (var fileStream = File.OpenRead("assets/testimage.png"))
            {
                var image = await imageFactory.Create(fileStream, "testimage.png", "image/png");

                Assert.NotNull(image);
            }
        }

        [Fact]
        public async Task LocalImageFactory_InvalidContentType_ThrowsException()
        {
            var imageFactory = new LocalImageFactory(_localImageFactoryOptions, _imageFactoryValidator);

            using (var fileStream = File.Open("assets/iisexpress.exe", FileMode.Open))
            {
                await Assert.ThrowsAsync<ArgumentException>(async () => await imageFactory.Create(fileStream, "iisexpress.exe", "application/octet-stream"));
            }
        }

        [Fact]
        public async Task LocalImageFactory_Empty_ThrowsException()
        {
            var imageFactory = new LocalImageFactory(_localImageFactoryOptions, _imageFactoryValidator);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await imageFactory.Create(Stream.Null, "test.png", "image/png");
            });
        }

        public void Dispose()
        {
            if (Directory.Exists(_uploadPath))
                Directory.Delete(_uploadPath, recursive: true);
        }
    }
}
