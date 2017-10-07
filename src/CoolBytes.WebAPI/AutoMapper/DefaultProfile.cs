using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Features.Images;

namespace CoolBytes.WebAPI.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<BlogPost, BlogPostSummaryViewModel>()
                .ForMember(v => v.AuthorName, exp => exp.MapFrom(b => b.Author.AuthorProfile.FirstName))
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost);
            CreateMap<BlogPost, BlogPostUpdateViewModel>()
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost);
            CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(v => v.Links, exp => exp.Ignore())
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost);
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

        private static void ResolveImageModelFromBlogPost<T>(IMemberConfigurationExpression<BlogPost, T, ImageViewModel> exp)
        {
            exp.ResolveUsing((blogPost, viewModel, image) =>
                                    blogPost.Image == null ? null : new ImageViewModel() { UriPath = blogPost.Image.UriPath });
        }
    }
}

