using FinalProject.Areas.Admin.DTOs;
using FinalProject.DTOs;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class ProductsListVm
    {
        public List<ProductDto> Products { get; set; }
       
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
