using FinalProject.DTOs;
using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Components.Home
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public SliderViewComponent(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = _context.Sliders.Select(s => new SliderDto
            {
                SliderId = s.Id,
                Title = s.Title,
                BackroundImageUrl = _configuration["Folders:Sliders"] + s.BackgroundImageUrl,
                Text = s.Slogan,
                Link = s.Link,

            }).OrderByDescending(s => s.SliderId).ToList();

            return View(sliders);
        }
    }
}
