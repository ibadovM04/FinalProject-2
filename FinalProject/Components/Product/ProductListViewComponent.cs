using FinalProject.Interfaces;
using FinalProject.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Components.Product
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly IProductManager _productManager;

        private readonly IConfiguration _configuration;
        public ProductListViewComponent(IConfiguration configuration, IProductManager productManager)
        {
            _configuration = configuration;
            _productManager = productManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(ProductListQueryModel request)
        {
            var vm = await _productManager.GetFilteredProducts(request);

            return View(vm);
        }
    }
}
