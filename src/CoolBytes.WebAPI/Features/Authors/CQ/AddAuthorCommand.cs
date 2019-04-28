using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Authors.DTO;
using CoolBytes.WebAPI.Features.Authors.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Authors.CQ
{
    public class AddAuthorCommand : IRequest<AuthorViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string About { get; set; }
        public int? ImageId { get; set; }
        public string ResumeUri { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public IEnumerable<ExperienceDto> Experiences { get; set; }
    }
}