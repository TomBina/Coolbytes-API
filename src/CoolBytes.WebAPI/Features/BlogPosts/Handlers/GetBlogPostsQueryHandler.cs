using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, IEnumerable<BlogPostSummaryViewModel>>
    {
        private readonly AppDbContext _context;

        public GetBlogPostsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPostSummaryViewModel>> Handle(GetBlogPostsQuery message, CancellationToken cancellationToken)
            => await ViewModel(message);

        private async Task<IEnumerable<BlogPostSummaryViewModel>> ViewModel(GetBlogPostsQuery message)
        {
            IEnumerable<BlogPost> blogPosts;

            if (message.Tag == null)
            {
                blogPosts = await QueryBlogPosts();
            }
            else
            {
                blogPosts = await QueryBlogPostsWithTag(message.Tag);
            }

            return Mapper.Map<IEnumerable<BlogPostSummaryViewModel>>(blogPosts);
        }

        private Task<List<BlogPost>> QueryBlogPostsWithTag(string tag) =>
            _context.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .Where(b => b.Tags.Any(t => t.Name == tag))
                .OrderByDescending(b => b.Id)
                .ToListAsync();

        private Task<List<BlogPost>> QueryBlogPosts() =>
            _context.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
    }
}
