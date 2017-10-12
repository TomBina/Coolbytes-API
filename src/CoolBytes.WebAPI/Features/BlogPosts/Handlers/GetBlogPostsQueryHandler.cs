using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class GetBlogPostsQueryHandler : IAsyncRequestHandler<GetBlogPostsQuery, IEnumerable<BlogPostSummaryViewModel>>
    {
        private readonly AppDbContext _context;

        public GetBlogPostsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPostSummaryViewModel>> Handle(GetBlogPostsQuery message) 
            => await ViewModel();

        private async Task<IEnumerable<BlogPostSummaryViewModel>> ViewModel()
        {
            var blogPosts = await GetBlogPosts();

            return Mapper.Map<IEnumerable<BlogPostSummaryViewModel>>(blogPosts);
        }

        private async Task<List<BlogPost>> GetBlogPosts() =>
            await _context.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
    }
}
