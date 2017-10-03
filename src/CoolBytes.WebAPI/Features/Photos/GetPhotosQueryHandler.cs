using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class GetPhotosQueryHandler : IAsyncRequestHandler<GetPhotosQuery, IEnumerable<PhotoViewModel>>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public GetPhotosQueryHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<PhotoViewModel>> Handle(GetPhotosQuery message)
        {
            var photos = await _context.Photos.ToListAsync();
            var viewModel = Mapper.Map<IEnumerable<PhotoViewModel>>(photos);

            foreach (var model in viewModel)
                model.FormatPhotoUri(_configuration);

            return viewModel;
        }
    }
}