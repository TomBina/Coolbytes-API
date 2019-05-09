using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Features.Images.CQ
{
    public class UploadImagesCommand : IRequest<IEnumerable<ImageViewModel>>
    {
        public List<IFormFile> Files { get; set; }
    }
}