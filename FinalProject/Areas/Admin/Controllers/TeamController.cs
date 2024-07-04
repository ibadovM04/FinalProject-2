using FinalProject.Areas.Admin.Models;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Helper;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private ICompositeViewEngine _viewEngine;

        public TeamController(ApplicationDbContext context,
                                         IConfiguration configuration,
                                         ICompositeViewEngine viewEngine)
        {
            _context = context;
            _configuration = configuration;
            _viewEngine = viewEngine;
        }
        [HttpGet]
        public async Task<IActionResult> List(int page = 1)
        {
            var query = _context.Teams.Where(u => true);

            var result = await SelectTeams(query, page);

            var vm = new TeamListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Teams = result.Item1
            };

            return View(vm);
        }
        public async Task<JsonResult> Filter([FromQuery] TeamFilterModel request)
        {


            var query = _context.Teams.Where(u =>
                                            (String.IsNullOrEmpty(request.TeamMemberName) || u.TeamMemberName.ToLower().StartsWith(request.TeamMemberName.ToLower())) &&
                                          (String.IsNullOrEmpty(request.TeamMemberPosition) || u.TeamMemberPosition.ToLower().StartsWith(request.TeamMemberPosition.ToLower())));




            var result = await SelectTeams(query, request.Page);


            var vm = new TeamListVm
            {
                CurrentPage = request.Page,
                TotalPage = result.Item2,
                Teams = result.Item1
            };

            var html = await RenderPartialView.InvokeAsync("_TeamListPartialView",
                                                            ControllerContext,
                                                            _viewEngine,
                                                            ViewData,
                                                            TempData,
                                                            vm);

            return Json(new
            {
                status = 200,
                data = html
            });
        }

        private async Task<(List<TeamDto>, int)> SelectTeams(IQueryable<Team> query, int page)
        {
            var takeNumber = Convert.ToInt32(_configuration["List:AdminTeams"]);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var contacts = await query
                                 .Select(u => new TeamDto
                                 {
                                     TeamMemberName = u.TeamMemberName,
                                     TeamMemberDescription = u.TeamMemberDescription,
                                     TeamMemberPosition = u.TeamMemberPosition,

                                 })
                                 .Skip((page - 1) * takeNumber)
                                 .Take(takeNumber)
                                 .ToListAsync();


            return (contacts, totalPage);
        }
    }
}
