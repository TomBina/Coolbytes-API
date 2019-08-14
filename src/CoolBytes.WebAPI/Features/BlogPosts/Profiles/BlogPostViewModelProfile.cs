using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;

namespace CoolBytes.WebAPI.Features.BlogPosts.Profiles
{
    public class BlogPostViewModelProfile : Profile
    {
        public BlogPostViewModelProfile()
        {
            CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(v => v.Updated, exp => exp.MapFrom(b => b.Content.Updated))
                .ForMember(v => v.Subject, exp => exp.MapFrom(b => b.Content.Subject))
                .ForMember(v => v.SubjectUrl, exp => exp.MapFrom(b => b.Content.SubjectUrl))
                .ForMember(v => v.ContentIntro, exp => exp.MapFrom(b => b.Content.ContentIntro))
                .ForMember(v => v.Content, exp => exp.MapFrom(b => b.Content.Content))
                .ForMember(v => v.Category, exp => exp.MapFrom(b => b.Category.Name))
                .ForMember(v => v.IsCourse, exp => exp.MapFrom(b => b.Category.IsCourse));
        }
    }
}
