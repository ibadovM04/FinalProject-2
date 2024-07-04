using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Components.Home
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public NavigationViewComponent(ApplicationDbContext context)
        {

            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var navCategories = _context.Categories.Where(c => c.ParentId == null).Select(c => new CategoryDto
            {
                CategoryId = c.Id,
                Name = c.Name,
                Priority = c.Priority ?? 0

            }).ToList();

            return View(navCategories);
        }
    }
}
