using CoolBytes.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class UploadPhotoCommand : IRequest<PhotoViewModel>
    {
        public IFormFile File { get; set; }
    }
}