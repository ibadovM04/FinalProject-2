﻿using FinalProject.Areas.Admin.DTOs;
using FinalProject.Areas.Admin.Models;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Data;
using FinalProject.Enums;
using FinalProject.Helper;
using FinalProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookies")]
    public class UserController : Controller
    {

        private ICompositeViewEngine _viewEngine;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context,
            ICompositeViewEngine viewEngine,
            IConfiguration configuration)
        {
            _context = context;
            _viewEngine = viewEngine;
            _configuration = configuration;

        }
        public async Task<IActionResult> List(int page = 1)
        {
            var query = _context.Users.Where(u => true);

            var result = await SelectUsers(query, page);

            var vm = new UserListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Users = result.Item1
            };

            return View(vm);
        }
        [HttpGet]
        public async Task<JsonResult> Filter([FromQuery] UserFilterModel request)
        {
            //TODO add validation

            var query = _context.Users.Where(u =>
                                            (String.IsNullOrEmpty(request.Name) || u.Name.ToLower().StartsWith(request.Name.ToLower())) &&
                                            (String.IsNullOrEmpty(request.Surname) || u.Surname.ToLower().StartsWith(request.Surname.ToLower())) &&
                                            (String.IsNullOrEmpty(request.Email) || u.Email.ToLower().StartsWith(request.Email.ToLower())));

            var result = await SelectUsers(query, request.Page);
            //TODO: deaktiv function

            var vm = new UserListVm
            {
                CurrentPage = request.Page,
                TotalPage = result.Item2,
                Users = result.Item1
            };

            var html = await RenderPartialView.InvokeAsync("_UserListPartialView",
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

        [HttpGet]
        public IActionResult Delete(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(p => p.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<JsonResult> ChangeStatus([FromBody] ChangeUserStatusModel request)
        {
            //TODO: validation

            var user = await _context.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            if (user is null)
            {
                return Json(new
                {
                    status = 400,
                    error = "Belə bir istifadəçi yoxdur."
                });
            }

            user.UserStatusId = request.StatusId;

            await _context.SaveChangesAsync();

            return Json(new
            {
                status = 200
            });

        }
        private async Task<(List<UserDto>, int)> SelectUsers(IQueryable<User> query, int page)
        {
            var takeNumber = Convert.ToInt32(_configuration["List:AdminUsers"]);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var users = await query.Include(u => u.UserRole)
                                 .Select(u => new UserDto
                                 {
                                     UserId = u.Id,
                                     Name = u.Name,
                                     Surname = u.Surname,
                                     Email = u.Email,
                                     Gender = u.Gender,
                                     GenderText = u.Gender != null ? ((bool)u.Gender ? "Kişi" : "Qadın") : "Qeyd olunmayıb",
                                     UserStatusId = u.UserStatusId,
                                     StatusText = u.UserStatusId == (byte)UserStatusEnum.Active ? "Aktiv" : "Deaktiv",
                                     Registered = u.Created,
                                     Role = u.UserRole.Name,
                                     RoleId = u.UserRoleId
                                 })
                                 .Skip((page - 1) * takeNumber)
                                 .Take(takeNumber)
                                 .ToListAsync();


            return (users, totalPage);
        }
    }
}
