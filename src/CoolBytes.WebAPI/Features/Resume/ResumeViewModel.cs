using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Authors;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;

namespace CoolBytes.WebAPI.Features.Resume
{
    public class ResumeViewModel
    {
        public AuthorViewModel Author { get; set; }
        public IDictionary<string, IEnumerable<ResumeEventViewModel>> ResumeEvents { get; set; }
    }
}