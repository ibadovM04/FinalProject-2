using FinalProject.Data;
using FinalProject.Enums;
using FinalProject.Interfaces;
using FinalProject.ServiceModels;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Services
{
    public class UserManager : IUserManager
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly HttpContext _httpContext;

        
        public UserManager(IHttpContextAccessor httpContextAccessor,
                             IConfiguration configuration,
                             ApplicationDbContext context)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
            _context = context;



        }

        public async Task<(bool result, string errorMessage)> Login(LoginModel request)
        {
            

            var user = await _context.Users
                .Include(c => c.UserRole)
                .Where(c => c.Email == request.Email && c.UserStatusId == (int)UserStatusEnum.Active)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return (false, "Email or password is not correct");
            }

            var result = user.CheckPassword(request.Password);
            if (!result)
            {
                return (true, "Email or password is not correct");
            }

            var claims = new List<Claim>
        {
            new Claim("Name", user.Name),
            new Claim("Surname", user.Surname),
            new Claim("Email", user.Email),
            new Claim("Id", user.Id.ToString()),
            new Claim("RoleId", user.UserRoleId.ToString()),
            new Claim(ClaimTypes.Role, user.Name)
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return (true, null);
        }
    }
}
