using CoolBytes.Tests.Web.Features.Authors;
using CoolBytes.WebAPI.Features.Photos;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Photos
{
    public class PhotosTests : TestBase, IClassFixture<Fixture>
    {
        public PhotosTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void ShouldUploadPhoto()
        {
            var photoFactory = CreatePhotoFactory();
            var handler = new UploadPhotoCommandHandler(Context, photoFactory, Fixture.Configuration);
            var file = CreateFileMock().Object;
            var message = new UploadPhotoCommand() { File = file };

            var result = handler.Handle(message);

            Assert.NotNull(result);
        }
    }
}
