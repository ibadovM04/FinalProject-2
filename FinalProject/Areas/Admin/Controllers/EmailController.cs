using FinalProject.Areas.Admin.GmailService;
using FinalProject.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailController : Controller
    {
        private readonly EmailService _emailService;

        public EmailController()
        {
            _emailService = new EmailService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailModel emailModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _emailService.SendEmail(emailModel);
                    ViewBag.Message = "Email sent successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                }
            }
            return View("Index");
        }
    }
}

