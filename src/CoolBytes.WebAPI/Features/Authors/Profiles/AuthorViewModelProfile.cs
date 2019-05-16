using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Authors.ViewModels;
using CoolBytes.WebAPI.Features.Images.ViewModels;

namespace CoolBytes.WebAPI.Features.Authors.Profiles
{
    public class AuthorViewModelProfile : Profile
    {
        public AuthorViewModelProfile()
        {
            CreateMap<Author, AuthorViewModel>()
                .ForMember(v => v.FirstName, exp => exp.MapFrom(a => a.AuthorProfile.FirstName))
                .ForMember(v => v.LastName, exp => exp.MapFrom(a => a.AuthorProfile.LastName))
                .ForMember(v => v.About, exp => exp.MapFrom(a => a.AuthorProfile.About))
                .ForMember(v => v.Experiences, exp => exp.MapFrom(a => a.AuthorProfile.Experiences))
                .ForMember(v => v.ResumeUri, exp => exp.MapFrom(a => a.AuthorProfile.ResumeUri))
                .ForMember(v => v.SocialHandles, exp => exp.MapFrom(a => a.AuthorProfile.SocialHandles))
                .ForMember(v => v.Image, exp => exp.MapFrom(a => a.AuthorProfile.Image));

        }
    }
}
