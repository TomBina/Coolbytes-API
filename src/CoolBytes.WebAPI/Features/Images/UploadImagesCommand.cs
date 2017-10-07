using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Features.Images
{
    public class UploadImagesCommand : IRequest<IEnumerable<ImageViewModel>>
    {
        public List<IFormFile> Files { get; set; }
    }
}