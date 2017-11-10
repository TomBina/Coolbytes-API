using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Images
{
    public class GetImagesQueryHandler : IAsyncRequestHandler<GetImagesQuery, IEnumerable<ImageViewModel>>
    {
        private readonly AppDbContext _context;

        public GetImagesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ImageViewModel>> Handle(GetImagesQuery message)
        {
            var images = await _context.Images.ToListAsync();
            var viewModel = Mapper.Map<IEnumerable<ImageViewModel>>(images);

            return viewModel;
        }
    }
}