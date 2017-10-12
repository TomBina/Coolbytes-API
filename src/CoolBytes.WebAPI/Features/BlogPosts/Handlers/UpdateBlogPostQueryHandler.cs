using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class UpdateBlogPostQueryHandler : IAsyncRequestHandler<UpdateBlogPostQuery, BlogPostUpdateViewModel>
    {
        private readonly AppDbContext _context;

        public UpdateBlogPostQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPostUpdateViewModel> Handle(UpdateBlogPostQuery query)
        {
            var blogPost = await _context.BlogPosts.Include(b => b.Tags).Include(b => b.Image).FirstOrDefaultAsync(b => b.Id == query.Id);
            return Mapper.Map<BlogPostUpdateViewModel>(blogPost);
        }
    }
}