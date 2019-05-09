using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Images.CQ;
using CoolBytes.WebAPI.Features.Images.Handlers;
using CoolBytes.WebAPI.Features.Images.Profiles;
using CoolBytes.WebAPI.Features.Images.Profiles.Resolvers;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Images
{
    public class LocalImagesTests : TestBase<TestContext>
    {
        public LocalImagesTests(TestContext testContext) : base(testContext)
        {
        }

        private IMapper CreateMapper()
        {
            var sp = TestContext.ServiceProviderBuilder.Add(s =>
                s.AddTransient<IImageViewModelUrlResolver, LocalImageViewModelUrlResolver>()
                    .AddTransient<ImageViewModelResolver>()).Build();
            var profiles = new Profile[] { new ImageViewModelProfile() };
            var mapper = TestContext.CreateMapper(profiles, sp);
            return mapper;
        }

        [Fact]
        public async Task ShouldGetAllImages()
        {
            await AddImage();

            var handlerContext = TestContext.CreateHandlerContext<IEnumerable<ImageViewModel>>(RequestDbContext, CreateMapper());
            var handler = new GetImagesQueryHandler(handlerContext);
            var message = new GetImagesQuery();

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task ShouldUploadImages()
        {
            var imageFactory = TestContext.CreateImageService();
            var handlerContext = TestContext.CreateHandlerContext<IEnumerable<ImageViewModel>>(RequestDbContext, CreateMapper());
            var handler = new UploadImagesCommandHandler(handlerContext, imageFactory);
            var file1 = TestContext.CreateFileMock().Object;
            var file2 = TestContext.CreateFileMock().Object;
            var files = new List<IFormFile>() { file1, file2 };
            var message = new UploadImagesCommand() { Files = files };

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, RequestDbContext.Images.Count());
        }

        [Fact]
        public async Task ShouldDeleteImage()
        {
            var image = await AddImage();

            IRequestHandler<DeleteImageCommand> handler = new DeleteImageCommandHandler(RequestDbContext, TestContext.CreateImageService());
            var message = new DeleteImageCommand() { Id = image.Id };

            await handler.Handle(message, CancellationToken.None);

            Assert.Equal(null, await RequestDbContext.Images.FindAsync(image.Id));
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
            RequestDbContext.Images.RemoveRange(RequestDbContext.Images.ToArray());
            await RequestDbContext.SaveChangesAsync();

            RequestDbContext.Dispose();
        }
    }
}