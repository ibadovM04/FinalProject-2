using FinalProject.Areas.Admin.Models;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Helper;
using FinalProject.Model;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private ICompositeViewEngine _viewEngine;

        public ContactController(ApplicationDbContext context,
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
            var query = _context.Contacts.Where(u => true);

            var result = await SelectContacts(query, page);

            var vm = new ContactListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Contacts = result.Item1
            };

            return View(vm);
        }
        public async Task<JsonResult> Filter([FromQuery] ContactFilterModel request)
        {
            

            var query = _context.Contacts.Where(u =>
                                            (String.IsNullOrEmpty(request.FirstName) || u.FirstName .ToLower().StartsWith(request.FirstName.ToLower())) &&
                                            (String.IsNullOrEmpty(request.LastName) || u.LastName.ToLower().StartsWith(request.LastName.ToLower())));




            var result = await SelectContacts(query, request.Page);
          

            var vm = new ContactListVm
            {
                CurrentPage = request.Page,
                TotalPage = result.Item2,
                Contacts = result.Item1
            };

            var html = await RenderPartialView.InvokeAsync("_ContactListPartialView",
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

        private async Task<(List<ContactDto>, int)> SelectContacts(IQueryable<Contact> query, int page)
        {
            var takeNumber = Convert.ToInt32(_configuration["List:AdminContacts"]);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var contacts = await query
                                 .Select(u => new ContactDto
                                 {
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Email = u.Email,
                                     Subject = u.Subject,
                                     Message = u.Message,


                                 })
                                 .Skip((page - 1) * takeNumber)
                                 .Take(takeNumber)
                                 .ToListAsync();


            return (contacts, totalPage);
        }
    }
}
