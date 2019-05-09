using AutoMapper;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using System;
using System.Collections.Generic;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostViewModelBuilder
    {
        private readonly IMapper _mapper;
        private BlogPostViewModel _model;

        public BlogPostViewModelBuilder(IMapper mapper)
        {
            _mapper = mapper;
        }

        public BlogPostViewModelBuilder FromBlog(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException();

            _model = _mapper.Map<BlogPostViewModel>(blogPost);

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