using FinalProject.Areas.Admin.ViewModels;
using FinalProject.DTOs;
using FinalProject.Model;
using FinalProject.ServiceModels;
using FinalProject.ViewModels;

namespace FinalProject.Interfaces
{
    public interface IProductManager
    {
        Task<ProductListVm> GetFilteredProducts(ProductListQueryModel request);
        Task<(bool, Dictionary<string, string>)> ValidateProduct(ProductPostModel request);

        Task<bool> CreateProduct(ProductPostModel request);

        Task<ProductDto> GetProductById(Guid productId);
        Product GetById(Guid Id);
        void DeleteProduct(Guid Id);
    }
}
