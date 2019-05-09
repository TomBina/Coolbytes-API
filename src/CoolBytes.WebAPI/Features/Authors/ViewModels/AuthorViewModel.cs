using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Images.ViewModels;

namespace CoolBytes.WebAPI.Features.Authors.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string About { get; set; }
        public string ResumeUri { get; set; }
        public SocialHandlesViewModel SocialHandles { get; set; }
        public ImageViewModel Image { get; set; }
        public IEnumerable<ExperienceViewModel> Experiences { get; set; }
    }
}