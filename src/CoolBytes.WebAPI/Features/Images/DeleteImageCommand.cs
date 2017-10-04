using MediatR;

namespace CoolBytes.WebAPI.Features.Images
{
    public class DeleteImageCommand : IRequest
    {
        public int Id { get; set; }
    }
}