using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject.Components.Home
{
    public class BigSaleProductViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public BigSaleProductViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bigSaleProducts = await _context
                                        .BigSaleProducts
                                        .Select(c => new BigSaleProductDto
                                        {
                                            Link = c.Link,
                                            Title = c.Title,
                                            ImageURL = _configuration["Folders:BigSaleProducts"] + c.ImageURL,
                                            DiscountInfo = c.DiscountInfo,
                                            Description = c.Description
                                        })
                                        .ToListAsync();

            return View(bigSaleProducts);

        }

    }
}
