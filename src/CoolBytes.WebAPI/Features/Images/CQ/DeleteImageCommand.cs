using MediatR;

namespace CoolBytes.WebAPI.Features.Images.CQ
{
    public class DeleteImageCommand : IRequest
    {
        public int Id { get; set; }
    }
}