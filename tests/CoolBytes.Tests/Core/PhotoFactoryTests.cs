using CoolBytes.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core;
using CoolBytes.Core.Factories;
using Microsoft.Win32.SafeHandles;
using Xunit;
using Xunit.Abstractions;

namespace CoolBytes.Tests.Core
{
    public class PhotoFactoryTests
    {
        private readonly ITestOutputHelper _output;
        private readonly string _uploadPath = Environment.CurrentDirectory + "/uploads";
        private readonly PhotoFactoryOptions _photoFactoryOptions;
        private readonly PhotoFactoryValidator _photoFactoryValidator;

        public PhotoFactoryTests(ITestOutputHelper output)
        {
            _output = output;
            _photoFactoryOptions = new PhotoFactoryOptions(_uploadPath);
            _photoFactoryValidator = new PhotoFactoryValidator();
        }

        [Fact]
        public async Task PhotoFactory_Valid_CreatesPhoto()
        {
            var photoFactory = new PhotoFactory(_photoFactoryOptions, _photoFactoryValidator);

            using (var fileStream = File.Open("assets/testimage.png", FileMode.Open))
            {
                var photo = await photoFactory.Create(fileStream, "testimage.png", "image/png");

                Assert.NotNull(photo);
            }
        }

        [Fact]
        public async Task PhotoFactory_InvalidContentType_ThrowsException()
        {
            var photoFactory = new PhotoFactory(_photoFactoryOptions, _photoFactoryValidator);

            using (var fileStream = File.Open("assets/iisexpress.exe", FileMode.Open))
            {
                await Assert.ThrowsAsync<ArgumentException>(async () => await photoFactory.Create(fileStream, "iisexpress.exe", "application/octet-stream"));
            }
        }

        [Fact]
        public async Task PhotoFactory_Empty_ThrowsException()
        {
            var photoFactory = new PhotoFactory(_photoFactoryOptions, _photoFactoryValidator);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await photoFactory.Create(Stream.Null, "test.png", "image/png");
            });
        }
    }
}
