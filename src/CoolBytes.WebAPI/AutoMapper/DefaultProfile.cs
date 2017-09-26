using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Features.BlogPosts;

namespace CoolBytes.WebAPI.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(v => v.AuthorName, exp => exp.MapFrom(b => b.Author.AuthorProfile.FirstName))
                .ForMember(v => v.Photo, exp => exp.MapFrom(b => b.Photo.FileName));
            CreateMap<BlogPostTag, BlogPostTagViewModel>();
            CreateMap<Author, AuthorViewModel>()
                .ForMember(v => v.FirstName, exp => exp.MapFrom(a => a.AuthorProfile.FirstName))
                .ForMember(v => v.LastName, exp => exp.MapFrom(a => a.AuthorProfile.LastName))
                .ForMember(v => v.About, exp => exp.MapFrom(a => a.AuthorProfile.About));
        }
    }
}

