using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class GetBlogPostsQueryHandler : IAsyncRequestHandler<GetBlogPostsQuery, IEnumerable<BlogPostViewModel>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public GetBlogPostsQueryHandler(AppDbContext context, IConfiguration configuration)
        {
            _appDbContext = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<BlogPostViewModel>> Handle(GetBlogPostsQuery message)
        {
            var blogPosts = await GetBlogPosts();

            return CreateViewModel(blogPosts);
        }

        private async Task<List<BlogPost>> GetBlogPosts()
        {
            var blogPosts = await _appDbContext.BlogPosts.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Photo)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
            return blogPosts;
        }

        private IEnumerable<BlogPostViewModel> CreateViewModel(List<BlogPost> blogPosts)
        {
            var blogPostsViewModel = Mapper.Map<IEnumerable<BlogPostViewModel>>(blogPosts);

            foreach (var blogPostViewModel in blogPostsViewModel)
                blogPostViewModel.Photo?.FormatPhotoUri(_configuration);

            return blogPostsViewModel;
        }
    }
}
