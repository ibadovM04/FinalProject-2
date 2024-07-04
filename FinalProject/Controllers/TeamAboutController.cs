using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class TeamAboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public  TeamAboutController(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var teamAbout = await _context.TeamAbouts.Select(c => new TeamAboutDto
            {
                Description = c.Description,
                Title = c.Title,
                ImageUrl = _configuration["Folders:BigSaleProducts"] + c.ImageUrl,

            }).ToListAsync();   
            return View(teamAbout);

        }
    }
}
