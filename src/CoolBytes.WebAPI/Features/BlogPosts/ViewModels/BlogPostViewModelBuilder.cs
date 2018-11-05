using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using System;
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostViewModelBuilder
    {
        private BlogPostViewModel _model;

        public BlogPostViewModelBuilder FromBlog(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException();

            _model = Mapper.Map<BlogPostViewModel>(blogPost);

            return this;
        }

        public BlogPostViewModelBuilder WithRelatedLinks(List<BlogPostLinkDto> links)
        {
            if (links == null)
                throw new ArgumentNullException();

            if (links.Count == 0)
                throw new ArgumentException(nameof(links));

            _model.RelatedLinks = links;

            return this;
        }

        public BlogPostViewModel Build() => _model;
    }
}