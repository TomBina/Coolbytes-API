using MediatR;
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.Authors
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