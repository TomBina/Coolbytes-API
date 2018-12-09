using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class GetBlogPostsQueryHandler : BaseHandler, IRequestHandler<GetBlogPostsQuery, IEnumerable<BlogPostSummaryViewModel>>
    {
        public GetBlogPostsQueryHandler(AppDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
        }

        public async Task<IEnumerable<BlogPostSummaryViewModel>> Handle(GetBlogPostsQuery message, CancellationToken cancellationToken)
        {
            IEnumerable<BlogPostSummaryViewModel> viewModel;

            if (message.Tag == null)
            {
                viewModel = await GetOrAddAsync(() => ViewModelAsync(null));
            }

            else
            {
                viewModel = await GetOrAddAsync(() => ViewModelAsync(message.Tag));
            }

            return viewModel;
        }

        private async Task<IEnumerable<BlogPostSummaryViewModel>> ViewModelAsync(string tag)
        {
            IEnumerable<BlogPost> blogPosts;

            if (tag == null)
            {
                blogPosts = await QueryBlogPosts();
            }
            else
            {
                blogPosts = await QueryBlogPostsWithTag(tag);
            }

            return Mapper.Map<IEnumerable<BlogPostSummaryViewModel>>(blogPosts);
        }

        private Task<List<BlogPost>> QueryBlogPostsWithTag(string tag) =>
            Context.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .Include(b => b.Category)
                .Where(b => b.Tags.Any(t => t.Name == tag))
                .OrderByDescending(b => b.Id)
                .ToListAsync();

        private Task<List<BlogPost>> QueryBlogPosts() =>
            Context.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .Include(b => b.Category)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
    }
}