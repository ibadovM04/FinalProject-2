using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Components.Home
{
    public class HomeFagViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public HomeFagViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var fags = await _context
                                        .HomeFags
                                        .Select(c => new HomeFagDto
                                        {
                                            Title = c.Title,

                                            Description = c.Description
                                        })
                                        .ToListAsync();

            return View(fags);

        }
    }
}
