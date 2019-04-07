using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Features.Images;

namespace CoolBytes.WebAPI.Features.BlogPosts.Profiles
{
    public class BlogPostSummaryViewModelProfile : Profile
    {
        public BlogPostSummaryViewModelProfile()
        {
            CreateMap<BlogPost, BlogPostSummaryViewModel>()
                .ForMember(v => v.AuthorName, exp => exp.MapFrom(b => b.Author.AuthorProfile.FirstName))
                .ForMember(v => v.Subject, exp => exp.MapFrom(b => b.Content.Subject))
                .ForMember(v => v.SubjectUrl, exp => exp.MapFrom(b => b.Content.SubjectUrl))
                .ForMember(v => v.ContentIntro, exp => exp.MapFrom(b => b.Content.ContentIntro))
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost)
                .ForMember(v => v.Category, exp => exp.MapFrom(b => b.Category.Name));
        }

        private static void ResolveImageModelFromBlogPost<T>(IMemberConfigurationExpression<BlogPost, T, ImageViewModel> exp)
        {
            exp.MapFrom((blogPost, viewModel, image) =>
                blogPost.Image == null ? null : new ImageViewModel() { UriPath = blogPost.Image.UriPath });
        }

    }
}
