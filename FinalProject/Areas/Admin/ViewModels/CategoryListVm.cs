using FinalProject.DTOs;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class CategoryListVm
    {
        public List<CategoryDto> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
