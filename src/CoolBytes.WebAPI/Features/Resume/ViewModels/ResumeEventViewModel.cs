using CoolBytes.WebAPI.Features.Resume.DTO;

namespace CoolBytes.WebAPI.Features.Resume.ViewModels
{
    public class ResumeEventViewModel
    {
        public int Id { get; set; }
        public DateRangeDto DateRange { get; set; }
        public string Message { get; set; }
    }
}