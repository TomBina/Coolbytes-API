using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Features.Images;
using CoolBytes.WebAPI.Features.Resume;
using CoolBytes.WebAPI.Features.ResumeEvents.DTO;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;

namespace CoolBytes.WebAPI.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            MapBlogPost();
            MapAuthor();
            MapImage();
            MapResumeEvent();
            MapResume();
        }

        private void MapResume()
        {
            CreateMap<Resume, ResumeViewModel>();
        }

        private void MapImage()
        {
            CreateMap<Image, ImageViewModel>()
                .ForMember(v => v.UriPath, exp => exp.MapFrom(p => p.UriPath));
        }

        private void MapResumeEvent()
        {
            CreateMap<ResumeEvent, ResumeEventViewModel>();
            CreateMap<DateRange, DateRangeDto>();
        }

        private void MapAuthor()
        {
            CreateMap<Author, AuthorViewModel>()
                .ForMember(v => v.FirstName, exp => exp.MapFrom(a => a.AuthorProfile.FirstName))
                .ForMember(v => v.LastName, exp => exp.MapFrom(a => a.AuthorProfile.LastName))
                .ForMember(v => v.About, exp => exp.MapFrom(a => a.AuthorProfile.About))
                .ForMember(v => v.Experiences, exp => exp.MapFrom(a => a.AuthorProfile.Experiences))
                .ForMember(v => v.ResumeUri, exp => exp.MapFrom(a => a.AuthorProfile.ResumeUri))
                .ForMember(v => v.SocialHandles, exp => exp.MapFrom(a => a.AuthorProfile.SocialHandles))
                .ForMember(v => v.Image,
                    exp => exp.ResolveUsing((author, viewModel, image) =>
                    {
                        if (author.AuthorProfile != null)
                        {
                            return author.AuthorProfile.Image == null
                                ? null
                                : new ImageViewModel()
                                {
                                    Id = author.AuthorProfile.Image.Id,
                                    UriPath = author.AuthorProfile.Image.UriPath
                                };
                        }
                        else
                        {
                            return null;
                        }
                    }));
            CreateMap<SocialHandles, SocialHandlesViewModel>();
            CreateMap<Experience, ExperienceViewModel>();
        }

        private void MapBlogPost()
        {
            CreateMap<BlogPost, BlogPostSummaryViewModel>()
                .ForMember(v => v.AuthorName, exp => exp.MapFrom(b => b.Author.AuthorProfile.FirstName))
                .ForMember(v => v.Subject, exp => exp.MapFrom(b => b.Content.Subject))
                .ForMember(v => v.SubjectUrl, exp => exp.MapFrom(b => b.Content.SubjectUrl))
                .ForMember(v => v.ContentIntro, exp => exp.MapFrom(b => b.Content.ContentIntro))
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost);
            CreateMap<BlogPost, BlogPostUpdateViewModel>()
                .ForMember(v => v.Updated, exp => exp.MapFrom(b => b.Content.Updated))
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost)
                .ForMember(v => v.Subject, exp => exp.MapFrom(b => b.Content.Subject))
                .ForMember(v => v.ContentIntro, exp => exp.MapFrom(b => b.Content.ContentIntro))
                .ForMember(v => v.Content, exp => exp.MapFrom(b => b.Content.Content))
                .ForMember(v => v.ExternalLinks, exp => exp.MapFrom(b => b.ExternalLinks));
            CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(v => v.Updated, exp => exp.MapFrom(b => b.Content.Updated))
                .ForMember(v => v.ExternalLinks, exp => exp.MapFrom(b => b.ExternalLinks))
                .ForMember(v => v.Image, ResolveImageModelFromBlogPost)
                .ForMember(v => v.Subject, exp => exp.MapFrom(b => b.Content.Subject))
                .ForMember(v => v.ContentIntro, exp => exp.MapFrom(b => b.Content.ContentIntro))
                .ForMember(v => v.Content, exp => exp.MapFrom(b => b.Content.Content))
                .ForMember(v => v.RelatedLinks, exp => exp.Ignore());
            CreateMap<BlogPostTag, BlogPostTagDto>();
            CreateMap<ExternalLink, ExternalLinkDto>();
        }

        private static void ResolveImageModelFromBlogPost<T>(IMemberConfigurationExpression<BlogPost, T, ImageViewModel> exp)
        {
            exp.ResolveUsing((blogPost, viewModel, image) =>
                                    blogPost.Image == null ? null : new ImageViewModel() { UriPath = blogPost.Image.UriPath });
        }
    }
}

