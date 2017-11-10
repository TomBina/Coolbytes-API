using System.Collections.Generic;
using MediatR;

namespace CoolBytes.WebAPI.Features.Images
{
    public class GetImagesQuery : IRequest<IEnumerable<ImageViewModel>>
    {
    }
}