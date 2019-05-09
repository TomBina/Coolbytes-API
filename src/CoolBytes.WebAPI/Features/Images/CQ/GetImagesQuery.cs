using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Images.CQ
{
    public class GetImagesQuery : IRequest<IEnumerable<ImageViewModel>>
    {
    }
}