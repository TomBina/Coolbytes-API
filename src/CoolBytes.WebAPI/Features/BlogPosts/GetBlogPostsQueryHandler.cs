using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class GetBlogPostsQueryHandler : IAsyncRequestHandler<GetBlogPostsQuery, IEnumerable<BlogPostViewModel>>
    {
        private readonly AppDbContext _appDbContext;

        public GetBlogPostsQueryHandler(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<IEnumerable<BlogPostViewModel>> Handle(GetBlogPostsQuery message)
        {
            var blogPosts = await _appDbContext.BlogPosts.AsNoTracking().Include(b => b.Author).Include(b => b.Author.AuthorProfile).ToListAsync();

            return Mapper.Map<IEnumerable<BlogPostViewModel>>(blogPosts);
        }
    }
}
