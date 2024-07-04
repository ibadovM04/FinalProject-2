using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class DiscountProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public DiscountProductController(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> List()
        {
            var discountProduct = await _context.Products.Where(c => c.Discount != null).Select(c => new ProductDto
            {
                ProductId = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ImageURL = _configuration["Folders:DiscountProducts"] + c.ProductPhotos.Where(a => a.IsMain).Select(a => a.ImageUrl).FirstOrDefault(),
                Price = c.SellAmount,
                Discount = c.Discount,
                DiscountedPrice = c.Discount != null ? c.SellAmount - (c.SellAmount * (double)c.Discount / 100) : null,
                Description = c.Description
            }).ToListAsync();
            return View(discountProduct);
            
        }
    }
}
