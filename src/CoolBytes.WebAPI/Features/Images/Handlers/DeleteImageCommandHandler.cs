using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Images.CQ;
using MediatR;

namespace CoolBytes.WebAPI.Features.Images.Handlers
{
    public class DeleteImageCommandHandler : AsyncRequestHandler<DeleteImageCommand>
    {
        private readonly AppDbContext _context;
        private readonly IImageService _imageService;

        public DeleteImageCommandHandler(AppDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        protected override async Task Handle(DeleteImageCommand message, CancellationToken cancellationToken)
        {
            var image = await _context.Images.FindAsync(message.Id);

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            await _imageService.Delete(image);
        }
    }
}