using FinalProject.DTOs;

namespace FinalProject.ViewModels
{
    public class HomeIndexVm
    {
        public List<SliderDto> Sliders { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<FeaturedProductDto> FeaturedProducts { get; set; }
        public List<BigSaleProductDto> BigSaleProducts { get; set; }

    }
}
