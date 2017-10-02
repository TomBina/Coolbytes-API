using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class UploadPhotoCommandHandler : IAsyncRequestHandler<UploadPhotoCommand, PhotoViewModel>
    {
        private readonly IPhotoFactory _photoFactory;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UploadPhotoCommandHandler(AppDbContext context, IPhotoFactory photoFactory, IConfiguration configuration)
        {
            _context = context;
            _photoFactory = photoFactory;
            _configuration = configuration;
        }

        public async Task<PhotoViewModel> Handle(UploadPhotoCommand message)
        {
            var photo = await CreatePhoto(message.File);
            await SavePhoto(photo);
            var viewModel = CreateViewModel(photo);

            return viewModel;
        }

        private async Task<Photo> CreatePhoto(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                return await _photoFactory.Create(stream, file.FileName, file.ContentType);
            }
        }
        private async Task SavePhoto(Photo photo)
        {
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync(() => File.Delete(photo.Path));
        }

        private PhotoViewModel CreateViewModel(Photo photo)
        {
            var viewModel = Mapper.Map<PhotoViewModel>(photo);
            viewModel.FormatPhotoUri(_configuration);
            return viewModel;
        }
    }
}