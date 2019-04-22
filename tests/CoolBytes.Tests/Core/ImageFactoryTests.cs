using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Services.ImageFactories;
using Xunit;

namespace CoolBytes.Tests.Core
{
    public class ImageFactoryTests : IDisposable
    {
        private readonly string _uploadPath = Environment.CurrentDirectory + "/uploads";
        private readonly ImageFactoryOptions _imageFactoryOptions;
        private readonly ImageFactoryValidator _imageFactoryValidator;

        public ImageFactoryTests()
        {
            _imageFactoryOptions = new ImageFactoryOptions(_uploadPath);
            _imageFactoryValidator = new ImageFactoryValidator();
        }

        [Fact]
        public async Task ImageFactory_Valid_CreatesImage()
        {
            var imageFactory = new LocalImageFactory(_imageFactoryOptions, _imageFactoryValidator);

            using (var fileStream = File.OpenRead("assets/testimage.png"))
            {
                var image = await imageFactory.Create(fileStream, "testimage.png", "image/png");

                Assert.NotNull(image);
            }
        }

        [Fact]
        public async Task ImageFactory_InvalidContentType_ThrowsException()
        {
            var imageFactory = new LocalImageFactory(_imageFactoryOptions, _imageFactoryValidator);

            using (var fileStream = File.Open("assets/iisexpress.exe", FileMode.Open))
            {
                await Assert.ThrowsAsync<ArgumentException>(async () => await imageFactory.Create(fileStream, "iisexpress.exe", "application/octet-stream"));
            }
        }

        [Fact]
        public async Task ImageFactory_Empty_ThrowsException()
        {
            var imageFactory = new LocalImageFactory(_imageFactoryOptions, _imageFactoryValidator);

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
