using CoolBytes.Tests.Web.Features.Authors;
using CoolBytes.WebAPI.Features.Photos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Photos
{
    public class PhotosTests : TestBase, IClassFixture<Fixture>, IAsyncLifetime
    {
        public PhotosTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ShouldGetAllPhotos()
        {
            await AddPhoto();

            var handler = new GetPhotosQueryHandler(Context, Fixture.Configuration);
            var message = new GetPhotosQuery();

            var result = await handler.Handle(message);

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task ShouldUploadPhotos()
        {
            var photoFactory = CreatePhotoFactory();
            var handler = new UploadPhotosCommandHandler(Context, photoFactory, Fixture.Configuration);
            var file1 = CreateFileMock().Object;
            var file2 = CreateFileMock().Object;
            var files = new List<IFormFile>() { file1, file2 };
            var message = new UploadPhotosCommand() { Files = files };

            var result = await handler.Handle(message);

            Assert.NotNull(result);
            Assert.Equal(2, Context.Photos.Count());
        }

        [Fact]
        public async Task ShouldDeletePhoto()
        {
            await AddPhoto();

            var handler = new DeletePhotoCommandHandler(Context);
            var message = new DeletePhotoCommand() { Id = 1 };

            await handler.Handle(message);

            Assert.Equal(0, Context.Photos.Count());
        }

        private async Task AddPhoto()
        {
            var photoFactory = CreatePhotoFactory();
            var file = CreateFileMock().Object;

            using (var stream = file.OpenReadStream())
            {
                var photo = await photoFactory.Create(stream, file.FileName, file.ContentType);
                using (var context = Fixture.CreateNewContext())
                {
                    context.Photos.Add(photo);
                    await context.SaveChangesAsync();
                }
            }
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            Context.Photos.RemoveRange(Context.Photos.ToArray());
            await Context.SaveChangesAsync();

            Context.Dispose();
        }
    }
}