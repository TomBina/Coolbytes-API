using System.Collections.Generic;
using CoolBytes.WebAPI.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Images
{
    public class GetImagesQuery : IRequest<IEnumerable<ImageViewModel>>
    {
    }
}