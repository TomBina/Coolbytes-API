using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;

namespace CoolBytes.WebAPI.Features.BlogPosts.Profiles
{
    public class BlogPostTagDtoProfile : Profile
    {
        public BlogPostTagDtoProfile()
        {
            CreateMap<BlogPostTag, BlogPostTagDto>();
        }
    }
}
