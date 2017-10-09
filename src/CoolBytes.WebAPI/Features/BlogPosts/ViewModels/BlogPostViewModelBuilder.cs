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

        public BlogPostViewModelBuilder FromBlog(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException();

            _model = Mapper.Map<BlogPostViewModel>(blogPost);

            return this;
        }

        public BlogPostViewModelBuilder WithRelatedLinks(List<BlogPostLinkViewModel> links)
        {
            if (links == null)
                throw new ArgumentNullException();

            if (links.Count == 0)
                throw new ArgumentException(nameof(links));

            _model.Links = links;

            return this;
        }

        public BlogPostViewModel Build() => _model;
    }
}