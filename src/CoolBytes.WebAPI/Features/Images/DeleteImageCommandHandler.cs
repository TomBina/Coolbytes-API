using System.IO;
using System.Threading.Tasks;
using CoolBytes.Data;
using MediatR;

namespace CoolBytes.WebAPI.Features.Images
{
    public class DeleteImageCommandHandler : IAsyncRequestHandler<DeleteImageCommand>
    {
        private readonly AppDbContext _context;

        public DeleteImageCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteImageCommand message)
        {
            var image = await _context.Images.FindAsync(message.Id);

            File.Delete(image.Path);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}