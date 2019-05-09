using AutoMapper;

namespace CoolBytes.Tests.Web.Features
{
    public class TestViewModelProfile : Profile
    {
        public TestViewModelProfile()
        {
            CreateMap<TestModel, TestViewModel>().ForMember(v => v.FullName,
                exp => exp.MapFrom(model => $"{model.FirstName} {model.LastName}"));
        }
    }
}