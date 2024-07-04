using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Components.Home
{
    public class FeaturedProductViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public FeaturedProductViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var featuredProducts = await _context
                                        .FeaturedProducts
                                        .Select(c => new FeaturedProductDto
                                        {
                                            ProductId = c.ProductId,
                                            Name = c.Name,
                                            Slug = c.Slug,
                                            ImageURL = _configuration["Folders:FeaturedProducts"] + c.ImageURL,
                                            Price = c.Price,
                                            Description = c.Description
                                        })
                                        .ToListAsync();

            return View(featuredProducts);

        }

    }
}
