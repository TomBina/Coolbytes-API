using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Images.CQ;
using CoolBytes.WebAPI.Features.Images.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Images
{
    public class ImagesTests : TestBase
    {
        public ImagesTests(TestContext testContext) : base(testContext)
        {
        }

        [Fact]
        public async Task ShouldGetAllImages()
        {
            await AddImage();

            var handler = new GetImagesQueryHandler(Context);
            var message = new GetImagesQuery();

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task ShouldUploadImages()
        {
            var imageFactory = TestContext.CreateImageService();
            var handler = new UploadImagesCommandHandler(Context, imageFactory);
            var file1 = TestContext.CreateFileMock().Object;
            var file2 = TestContext.CreateFileMock().Object;
            var files = new List<IFormFile>() { file1, file2 };
            var message = new UploadImagesCommand() { Files = files };

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, Context.Images.Count());
        }

        [Fact]
        public async Task ShouldDeleteImage()
        {
            var image = await AddImage();

            IRequestHandler<DeleteImageCommand> handler = new DeleteImageCommandHandler(Context, TestContext.CreateImageService());
            var message = new DeleteImageCommand() { Id = image.Id };

            await handler.Handle(message, CancellationToken.None);

            Assert.Equal(null, await Context.Images.FindAsync(image.Id));
        }

        private async Task<Image> AddImage()
        {
            var imageService = TestContext.CreateImageService();
            var file = TestContext.CreateFileMock().Object;

            using (var stream = file.OpenReadStream())
            {
                var image = await imageService.Save(stream, file.FileName, file.ContentType);
                using (var context = TestContext.CreateNewContext())
                {
                    context.Images.Add(image);
                    await context.SaveChangesAsync();
                    return image;
                }
            }
        }

        public override async Task DisposeAsync()
        {
            Context.Images.RemoveRange(Context.Images.ToArray());
            await Context.SaveChangesAsync();

            Context.Dispose();
        }
    }
}