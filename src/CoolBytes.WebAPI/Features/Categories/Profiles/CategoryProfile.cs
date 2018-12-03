using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Categories.ViewModels;

namespace CoolBytes.WebAPI.Features.Categories.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>().ForMember(cv => cv.CategoryId, opt => opt.MapFrom(c => c.Id));
        }
    }
}
