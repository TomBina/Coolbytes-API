using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Data;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.Images
{
    public class DeleteImageCommandHandler : AsyncRequestHandler<DeleteImageCommand>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public DeleteImageCommandHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        protected override async Task Handle(DeleteImageCommand message, CancellationToken cancellationToken)
        {
            var image = await _context.Images.FindAsync(message.Id);

            File.Delete($"{_configuration["ImagesUploadPath"]}{image.Path}");
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}