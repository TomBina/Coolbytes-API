using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Features.Images.ViewModels;

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
                .ForMember(v => v.Image, exp => exp.Ignore())
                .ForMember(v => v.Category, exp => exp.MapFrom(b => b.Category.Name))
                .AfterMap<AfterMapAction>();
                
        }
    }

    public class AfterMapAction : IMappingAction<BlogPost, BlogPostSummaryViewModel>
    {
        private readonly IImageViewModelFactory _factory;

        public AfterMapAction(IImageViewModelFactory factory)
        {
            _factory = factory;
        }

        public void Process(BlogPost source, BlogPostSummaryViewModel destination)
            => destination.Image = _factory.Create(source?.Image);
    }

}
