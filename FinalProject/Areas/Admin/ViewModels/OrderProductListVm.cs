using FinalProject.DTOs;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class OrderProductListVm
    {
        
        public List<ProductDto> Carts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
