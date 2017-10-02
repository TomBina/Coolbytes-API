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
                .ForMember(v => v.Photo,
                    exp => exp.ResolveUsing((blogPost, viewModel, photo) => 
                                                blogPost.Photo == null ? null : new PhotoViewModel() { PhotoUriPath = blogPost.Photo.UriPath }));
            CreateMap<BlogPostTag, BlogPostTagViewModel>();
            CreateMap<Author, AuthorViewModel>()
                .ForMember(v => v.FirstName, exp => exp.MapFrom(a => a.AuthorProfile.FirstName))
                .ForMember(v => v.LastName, exp => exp.MapFrom(a => a.AuthorProfile.LastName))
                .ForMember(v => v.About, exp => exp.MapFrom(a => a.AuthorProfile.About))
                .ForMember(v => v.Photo, 
                    exp => exp.ResolveUsing((author, viewModel, photo) => 
                                                author.AuthorProfile.Photo == null ? null : new PhotoViewModel() { PhotoUriPath = author.AuthorProfile.Photo.UriPath }));
            CreateMap<Photo, PhotoViewModel>()
                .ForMember(v => v.PhotoUriPath, exp => exp.MapFrom(p => p.UriPath))
                .ForMember(v => v.PhotoUri, exp => exp.Ignore());
        }
    }
}

