using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Images.CQ;
using CoolBytes.WebAPI.Features.Images.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Images.Handlers
{
    public class GetImagesQueryHandler : IRequestHandler<GetImagesQuery, IEnumerable<ImageViewModel>>
    {
        private readonly HandlerContext<IEnumerable<ImageViewModel>> _context;
        private readonly AppDbContext _dbContext;

        public GetImagesQueryHandler(HandlerContext<IEnumerable<ImageViewModel>> context)
        {
            _context = context;
            _dbContext = context.DbContext;
        }

        public async Task<IEnumerable<ImageViewModel>> Handle(GetImagesQuery message, CancellationToken cancellationToken)
        {
            var images = await _dbContext.Images.ToListAsync();
            var viewModel = _context.Map(images);

            return viewModel;
        }
    }
}