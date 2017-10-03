using System.IO;
using System.Threading.Tasks;
using CoolBytes.Data;
using MediatR;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class DeletePhotoCommandHandler : IAsyncRequestHandler<DeletePhotoCommand>
    {
        private readonly AppDbContext _context;

        public DeletePhotoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePhotoCommand message)
        {
            var photo = await _context.Photos.FindAsync(message.Id);

            File.Delete(photo.Path);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }
    }
}