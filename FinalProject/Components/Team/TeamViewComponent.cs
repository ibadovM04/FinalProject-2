using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Components.TeamAbout
{
    public class TeamViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public TeamViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var team = await _context.Teams.Select(c => new TeamDto
            {
                
                TeamMemberName = c.TeamMemberName,
                TeamMemberDescription = c.TeamMemberDescription,
                TeamMemberPosition = c.TeamMemberPosition,               
                profileImageUrl = _configuration["Folders:ProfileImages"] + c.profileImageUrl
             

            }).ToListAsync();
            return View(team);

        }
    }
}
