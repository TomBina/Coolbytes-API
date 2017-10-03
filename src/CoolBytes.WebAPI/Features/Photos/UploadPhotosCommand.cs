using System.Collections.Generic;
using CoolBytes.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class UploadPhotosCommand : IRequest<IEnumerable<PhotoViewModel>>
    {
        public List<IFormFile> Files { get; set; }
    }
}