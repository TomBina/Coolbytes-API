using CoolBytes.WebAPI.Features.Authors.ViewModels;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.Resume
{
    public class ResumeViewModel
    {
        public AuthorViewModel Author { get; set; }
        public IDictionary<string, IEnumerable<ResumeEventViewModel>> ResumeEvents { get; set; }
    }
}