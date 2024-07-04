using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject.Components.Home
{
    public class HomeCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public HomeCategoryViewComponent(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _context.Categories.Where(c => c.IsMainPage).Select(c => new CategoryDto
            {

                Slogan = c.Slogan,
                CategoryId = c.Id,
                Name = c.Name,
                ImageUrl = _configuration["Folders:Categories"] + c.ImageUrl,
                Priority = c.Priority ?? 0
            }).OrderBy(c => c.Priority).ToList();

            return View(categories);
        }


    }
}
