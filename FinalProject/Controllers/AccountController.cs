using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ServiceModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FinalProject.Enums;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using FinalProject.Model;
using FinalProject.Interfaces;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using FinalProject.Notifications.Slack;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;

        private readonly ApplicationDbContext _context;
        private readonly SlackService _slackService;
        public AccountController(ApplicationDbContext context, IUserManager userManager,  SlackService slackService)
        {
            _context = context;
            _userManager = userManager;
            _slackService = slackService;

        }

        public IActionResult Login()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel request)
        {
            //TODO: model state check
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var (result, errorMessage) = await _userManager.Login(request);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(request);
            }

            return RedirectToAction("Index","Home");
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel request)
        {

            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (!ModelState.IsValid)
                {
                    return View(request);
                }
                var user = await _context.Users.Where(c => c.Email == request.Email).FirstOrDefaultAsync();
                if (user is not null)
                {
                    ModelState.AddModelError("", "There is already user like this");
                    return View(request);
                }


                user = new User(request.Name,
                                request.Surname,
                                request.Email);

                user.IP = ip ?? "::00";
                user.AddPassword(request.Password);
                user.AddUserRole();

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
               
                _slackService.SendRegisterUserInfo(user.Name + " " + user.Surname + " " + user.Email + " " + user.Created?.ToString("dd:MM:yyyy hh:mm:ss") + " tarixində qeydiyyatdan keçmişdir.");
                return RedirectToAction("Login", "Account");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Internal", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email is not correct");

                return View("ForgotPassword", email);
            }

            var user = await _context.Users.Where(c => c.Email == email && c.UserStatusId == (int)UserStatusEnum.Active).FirstOrDefaultAsync();
            if (user is null)
            {
                ModelState.AddModelError("", "There is not any user like that");
                return View("ForgotPassword", email);

            }

            var otp = GenerateOTP();
            HttpContext.Session.SetString("otp", otp);
            HttpContext.Session.SetString("otp_userId", user.Id.ToString());

            string fromEmail = "taceddin_guven@hotmail.com";

            string password = "Melek2012";

            string toEmail = user.Email;

            string smtpAddress = "smtp.office365.com";

            int portNumber = 587;


            SmtpClient client = new SmtpClient(smtpAddress, portNumber);

            client.Credentials = new NetworkCredential(fromEmail, password);

            client.EnableSsl = true;


            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);


            mailMessage.Subject = "Şifrəni Yeniləmək üçün OTP";

            mailMessage.Body = otp;


            try
            {
                // Send the email
                client.SendMailAsync(mailMessage);

            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("ApproveOtp", "Account");


        }

        //[HttpPost]
        //public IActionResult SignOut()
        //{
        //    // Delete the cookie
        //    Response.Cookies.Delete("YourCookieName"); // Replace "YourCookieName" with your actual cookie name

        //    // Redirect to the home page or any other page
        //    return RedirectToAction("Index", "Home");
        //}



        [HttpGet]
        public async Task<IActionResult> ApproveOtp()
        {
            var oneTimePassword = HttpContext.Session.GetString("otp");
            if (oneTimePassword is null)
            {
                RedirectToAction("Index", "Home");
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOtp(int otpFromEmail)
        {
            var oneTimePassword = HttpContext.Session.GetString("otp");
            if (oneTimePassword is null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (oneTimePassword != otpFromEmail.ToString())
            {
                ModelState.AddModelError("", "OTP is not correct");
                return View("ApproveOtp", otpFromEmail);
            }

            HttpContext.Session.SetString("otp_approved", "true");



            return RedirectToAction("NewPassword", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> NewPassword()
        {
            var otpApproved = HttpContext.Session.GetString("otp_approved");
            if (otpApproved is null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> NewPassword(UpdatePasswordModel request)
        {
            var otpApproved = HttpContext.Session.GetString("otp_approved");
            if (otpApproved is null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var userId = HttpContext.Session.GetString("otp_userId");
            var user = await _context.Users.Where(c => c.Id.ToString() == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                ModelState.AddModelError("", "There is not any user like that");
                return View(request);
            }

            user.UpdatePassword(request.NewPassword);
            await _context.SaveChangesAsync();


            HttpContext.Session.Remove("otp");
            HttpContext.Session.Remove("otp_userId");
            HttpContext.Session.Remove("otp_approved");


            return RedirectToAction("Index", "Home");



        }



        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}

