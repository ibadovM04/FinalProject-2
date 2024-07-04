using FinalProject.DTOs;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class FaqListVm
    {
        public List<FaqDto> Faqs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
