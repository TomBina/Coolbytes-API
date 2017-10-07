using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class GetBlogPostsQueryHandler : IAsyncRequestHandler<GetBlogPostsQuery, IEnumerable<BlogPostSummaryViewModel>>
    {
        private readonly AppDbContext _appDbContext;

        public GetBlogPostsQueryHandler(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<IEnumerable<BlogPostSummaryViewModel>> Handle(GetBlogPostsQuery message)
        {
            var blogPosts = await GetBlogPosts();

            return CreateViewModel(blogPosts);
        }

        private async Task<List<BlogPost>> GetBlogPosts() => 
            await _appDbContext.BlogPosts
                        .AsNoTracking()
                        .Include(b => b.Author)
                        .Include(b => b.Author.AuthorProfile)
                        .Include(b => b.Image)
                        .OrderByDescending(b => b.Id)
                        .ToListAsync();

        private IEnumerable<BlogPostSummaryViewModel> CreateViewModel(List<BlogPost> blogPosts) => Mapper.Map<IEnumerable<BlogPostSummaryViewModel>>(blogPosts);
    }
}
