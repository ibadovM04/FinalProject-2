using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class FagController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public FagController(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var FAG = await _context
                                        .Fags
                                        .Select(c => new FaqDto
                                        {
                                            Title = c.QuestionsTitle,
                                            Description = c.QuestionsDescription
                                        })
                                        .ToListAsync();

            return View(FAG);
        }
    }
}
