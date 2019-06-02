namespace CoolBytes.WebAPI.Features.Categories.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
    }
}