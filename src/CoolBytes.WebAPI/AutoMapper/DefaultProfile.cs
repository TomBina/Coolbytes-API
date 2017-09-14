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
                .ForMember(b => b.AuthorName, exp => exp.MapFrom(b => b.Author.AuthorProfile.FirstName));
            CreateMap<Author, AuthorViewModel>()
                .ForMember(a => a.FirstName, exp => exp.MapFrom(a => a.AuthorProfile.FirstName))
                .ForMember(a => a.LastName, exp => exp.MapFrom(a => a.AuthorProfile.LastName))
                .ForMember(a => a.About, exp => exp.MapFrom(a => a.AuthorProfile.About));
        }
    }
}

