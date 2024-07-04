using FinalProject.Areas.Admin.Models;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Helper;
using FinalProject.Interfaces;
using FinalProject.Model;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FaqController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private ICompositeViewEngine _viewEngine;

        public FaqController(ApplicationDbContext context,
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
            var query = _context.Fags.Where(u => true);

            var result = await SelectFaq(query, page);

            var vm = new FaqListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Faqs = result.Item1
            };

            return View(vm);
        }
        public async Task<JsonResult> Filter([FromQuery] FaqFilterModel request)
        {


            var query = _context.Fags.Where(u =>
                                            (String.IsNullOrEmpty(request.Title) || u.QuestionsTitle.ToLower().StartsWith(request.Title.ToLower())) &&
                                            (String.IsNullOrEmpty(request.Description) || u.QuestionsDescription.ToLower().StartsWith(request.Description.ToLower())));




            var result = await SelectFaq(query, request.Page);


            var vm = new FaqListVm
            {
                CurrentPage = request.Page,
                TotalPage = result.Item2,
                Faqs = result.Item1
            };

            var html = await RenderPartialView.InvokeAsync("_FaqListPartialView",
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

        private async Task<(List<FaqDto>, int)> SelectFaq(IQueryable<FAG> query, int page)
        {
            var takeNumber = Convert.ToInt32(_configuration["List:AdminFaq"]);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var faqs = await query
                                 .Select(u => new FaqDto
                                 {
                                    Title=u.QuestionsTitle,
                                    Description=u.QuestionsDescription,

                                 })
                                 .Skip((page - 1) * takeNumber)
                                 .Take(takeNumber)
                                 .ToListAsync();


            return (faqs, totalPage);
        }
    }
}