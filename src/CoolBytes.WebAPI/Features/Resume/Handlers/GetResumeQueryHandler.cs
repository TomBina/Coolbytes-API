using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Resume.CQ;
using CoolBytes.WebAPI.Features.Resume.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Resume.Handlers
{
    public class GetResumeQueryHandler : IAsyncRequestHandler<GetResumeQuery, IEnumerable<ResumeEventViewModel>>
    {
        private readonly AppDbContext _context;
        private readonly IAuthorService _authorService;

        public GetResumeQueryHandler(AppDbContext context, IAuthorService authorService)
        {
            _context = context;
            _authorService = authorService;
        }

        public async Task<IEnumerable<ResumeEventViewModel>> Handle(GetResumeQuery message)
        {
            var author = await _authorService.GetAuthor();
            var resumeEvents = await _context.ResumeEvents.Where(r => r.Author == author).ToListAsync();

            return Mapper.Map<IEnumerable<ResumeEventViewModel>>(resumeEvents);
        }
    }
}