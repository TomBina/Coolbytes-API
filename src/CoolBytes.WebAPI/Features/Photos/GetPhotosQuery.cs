using System.Collections.Generic;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class GetPhotosQuery : IRequest<IEnumerable<PhotoViewModel>>
    {
    }
}