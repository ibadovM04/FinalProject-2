using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Roles = "Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }

       
    }
}
