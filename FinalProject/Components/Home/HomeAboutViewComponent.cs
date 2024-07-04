using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Components.Home
{
    public class HomeAboutViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;


        public HomeAboutViewComponent(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var homeAbout = await _context
                                        .HomeAbouts
                                        .Select(c => new HomeAboutDto
                                        {
                                            Title = c.Title,
                                            Description = c.Description
                                        })
                                        .ToListAsync();

            return View(homeAbout);

        }


    }
}
