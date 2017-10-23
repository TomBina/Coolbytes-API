using CoolBytes.WebAPI.Features.ResumeEvents.DTO;

namespace CoolBytes.WebAPI.Features.ResumeEvents.ViewModels
{
    public class ResumeEventViewModel
    {
        public int Id { get; set; }
        public DateRangeDto DateRange { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}