using CoolBytes.Core.Models;
using CoolBytes.Tests.Web.Features.Authors;
using CoolBytes.WebAPI.Features.Images;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Images
{
    public class ImagesTests : TestBase, IClassFixture<Fixture>, IAsyncLifetime
    {
        public ImagesTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ShouldGetAllImages()
        {
            await AddImage();

            var handler = new GetImagesQueryHandler(Context);
            var message = new GetImagesQuery();

            var result = await handler.Handle(message);

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task ShouldUploadImages()
        {
            var imageFactory = CreateImageFactory();
            var handler = new UploadImagesCommandHandler(Context, imageFactory);
            var file1 = CreateFileMock().Object;
            var file2 = CreateFileMock().Object;
            var files = new List<IFormFile>() { file1, file2 };
            var message = new UploadImagesCommand() { Files = files };

            var result = await handler.Handle(message);

            Assert.NotNull(result);
            Assert.Equal(2, Context.Images.Count());
        }

        [Fact]
        public async Task ShouldDeleteImage()
        {
            var image = await AddImage();

            var handler = new DeleteImageCommandHandler(Context);
            var message = new DeleteImageCommand() { Id = image.Id };

            await handler.Handle(message);

            Assert.Equal(null, await Context.Images.FindAsync(image.Id));
        }

        private async Task<Image> AddImage()
        {
            var imageFactory = CreateImageFactory();
            var file = CreateFileMock().Object;

            using (var stream = file.OpenReadStream())
            {
                var image = await imageFactory.Create(stream, file.FileName, file.ContentType);
                using (var context = Fixture.CreateNewContext())
                {
                    context.Images.Add(image);
                    await context.SaveChangesAsync();
                    return image;
                }
            }
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            Context.Images.RemoveRange(Context.Images.ToArray());
            await Context.SaveChangesAsync();

            Context.Dispose();
        }
    }
}