using FinalProject.Areas.Admin.ViewModels;

namespace FinalProject.DTOs;
    public class ProductDto
    {
    public Guid ProductId { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public string ImageURL { get; set; }
    public List<string> OtherImages { get; set; } = new List<string>();
    public string Description { get; set; }
    public double Price { get; set; }
    public bool IsWishlist { get; set; }
    public string? Barcode { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }

    public int? Discount { get; set; }

    public double? DiscountedPrice { get; set; }

    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
    public List<ProductSpecificationValue> Specifications { get; set; }

}

