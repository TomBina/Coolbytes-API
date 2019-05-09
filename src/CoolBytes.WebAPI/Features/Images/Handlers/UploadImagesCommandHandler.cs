using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Images.CQ;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Images.Handlers
{
    public class UploadImagesCommandHandler : IRequestHandler<UploadImagesCommand, IEnumerable<ImageViewModel>>
    {
        private readonly AppDbContext _dbContext;
        private readonly HandlerContext<IEnumerable<ImageViewModel>> _context;
        private readonly IImageService _imageService;

        public UploadImagesCommandHandler(HandlerContext<IEnumerable<ImageViewModel>> context, IImageService imageService)
        {
            _dbContext = context.DbContext;
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<ImageViewModel>> Handle(UploadImagesCommand message, CancellationToken cancellationToken)
        {
            var viewModel = new List<ImageViewModel>();

            foreach (var file in message.Files)
            {
                var image = await CreateImage(file);
                await SaveImage(image);
                var viewModelItem = CreateViewModel(image);

                viewModel.Add(viewModelItem);
            }

            return viewModel;
        }

        private async Task<Image> CreateImage(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                return await _imageService.Save(stream, file.FileName, file.ContentType);
            }
        }
        private async Task SaveImage(Image image)
        {
            _dbContext.Images.Add(image);
            await _dbContext.SaveChangesAsync(() => File.Delete(image.Path));
        }

        private ImageViewModel CreateViewModel(Image image) => _context.Mapper.Map<ImageViewModel>(image);
    }
}