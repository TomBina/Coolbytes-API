using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Features.BlogPosts;
using CoolBytes.WebAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(v => v.AuthorName, exp => exp.MapFrom(b => b.Author.AuthorProfile.FirstName))
                .ForMember(v => v.Image,
                    exp => exp.ResolveUsing((blogPost, viewModel, image) => 
                                                blogPost.Image == null ? null : new ImageViewModel() { UriPath = blogPost.Image.UriPath }));
            CreateMap<BlogPostTag, BlogPostTagViewModel>();
            CreateMap<Author, AuthorViewModel>()
                .ForMember(v => v.FirstName, exp => exp.MapFrom(a => a.AuthorProfile.FirstName))
                .ForMember(v => v.LastName, exp => exp.MapFrom(a => a.AuthorProfile.LastName))
                .ForMember(v => v.About, exp => exp.MapFrom(a => a.AuthorProfile.About))
                .ForMember(v => v.Image, 
                    exp => exp.ResolveUsing((author, viewModel, image) => 
                                                author.AuthorProfile.Image == null ? null : new ImageViewModel() { UriPath = author.AuthorProfile.Image.UriPath }));
            CreateMap<Image, ImageViewModel>()
                .ForMember(v => v.UriPath, exp => exp.MapFrom(p => p.UriPath));
        }
    }
}

