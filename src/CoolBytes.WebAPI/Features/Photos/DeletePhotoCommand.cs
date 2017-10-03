using MediatR;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class DeletePhotoCommand : IRequest
    {
        public int Id { get; set; }
    }
}