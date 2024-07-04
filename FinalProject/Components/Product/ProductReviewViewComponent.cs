using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Components.Product
{
    public class ProductReviewViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public ProductReviewViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var productReview = await _context.ProductReviews.Select(c => new ProductReviewDto
            {
               
                Message = c.Text,


            }).ToListAsync();
            return View(productReview);

        }
    }
}
