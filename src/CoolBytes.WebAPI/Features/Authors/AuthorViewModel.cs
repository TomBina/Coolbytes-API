using CoolBytes.WebAPI.Features.Images.ViewModels;
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.Authors
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