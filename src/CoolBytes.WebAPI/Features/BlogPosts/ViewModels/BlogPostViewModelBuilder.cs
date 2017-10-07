using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostViewModelBuilder
    {
        private readonly AppDbContext _context;
        private BlogPostViewModel _model;

        public BlogPostViewModelBuilder(AppDbContext context) => _context = context;

        public async Task<BlogPostViewModelBuilder> FromBlog(int id)
        {
            var blogPost = await _context.BlogPosts.AsNoTracking()
                                                   .Include(b => b.Author.AuthorProfile)
                                                   .Include(b => b.Tags)
                                                   .Include(b => b.Image)
                                                   .FirstOrDefaultAsync(b => b.Id == id);

            _model = Mapper.Map<BlogPostViewModel>(blogPost);

            return this;
        }

        public async Task<BlogPostViewModelBuilder> WithRelatedLinks()
        {
            if (_model == null)
                throw new InvalidOperationException();

            _model.Links = await _context.BlogPosts.Where(b => b.Id != _model.Id)
                .OrderByDescending(b => b.Id).Take(10).Select(b => new BlogPostLinkViewModel() { Id = b.Id, Date = b.Date, Subject = b.Subject }).ToListAsync();

            return this;
        }

        public BlogPostViewModel Build() => _model;
    }
}