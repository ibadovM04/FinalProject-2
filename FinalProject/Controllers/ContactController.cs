using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Models;
using FinalProject.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public ContactController(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var contact = new Contact();
            contact.Subject = request.Subject;
            contact.Email = request.Email;
            contact.Message = request.Message;
            contact.FirstName = request.FirstName;
            contact.LastName = request.LastName;

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");


        }


    }
}
