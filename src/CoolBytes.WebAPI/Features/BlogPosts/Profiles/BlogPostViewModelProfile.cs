using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Features.Images.ViewModels;

namespace CoolBytes.WebAPI.Features.BlogPosts.Profiles
{
    public class BlogPostViewModelProfile : Profile
    {
        public BlogPostViewModelProfile()
        {
            CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(v => v.Updated, exp => exp.MapFrom(b => b.Content.Updated))
                .ForMember(v => v.ExternalLinks, exp => exp.MapFrom(b => b.ExternalLinks))
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost)
                .ForMember(v => v.Subject, exp => exp.MapFrom(b => b.Content.Subject))
                .ForMember(v => v.SubjectUrl, exp => exp.MapFrom(b => b.Content.SubjectUrl))
                .ForMember(v => v.ContentIntro, exp => exp.MapFrom(b => b.Content.ContentIntro))
                .ForMember(v => v.Content, exp => exp.MapFrom(b => b.Content.Content))
                .ForMember(v => v.RelatedLinks, exp => exp.Ignore())
                .ForMember(v => v.Category, exp => exp.MapFrom(b => b.Category.Name));
        }

        private static void ResolveImageModelFromBlogPost<T>(IMemberConfigurationExpression<BlogPost, T, ImageViewModel> exp)
        {
            exp.MapFrom((blogPost, viewModel, image) =>
                blogPost.Image == null ? null : new ImageViewModel() { UriPath = blogPost.Image.UriPath });
        }
    }
}
