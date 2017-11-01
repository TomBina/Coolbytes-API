using CoolBytes.WebAPI.Features.Images;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class ExperienceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public ImageViewModel Image { get; set; }
    }
}