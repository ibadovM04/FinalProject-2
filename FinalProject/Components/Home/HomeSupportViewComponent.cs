using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Components.Home
{
    public class HomeSupportViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public HomeSupportViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var supports = await _context
                                        .Supports
                                        .Select(c => new SupportDto
                                        {
                                            Title = c.Title,

                                            Description = c.Description
                                        })
                                        .ToListAsync();

            return View(supports);

        }
    }
}
