using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.WebUI.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager,signInManager, roleManager)
        {
                
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await service.SignInAsync(model);
                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong username or password. Please try again.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Incorrect username or password");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await service.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user  = mapper.Map<User>(model);
                var result = await service.AddUserAsync(user, model.Password);
                if (result.Succeeded)
                {
                    TempData["Message"] = "User was registered successfully!";
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Index()
        {
            var user = userManager.GetUserAsync(User).Result;
            if (userManager.IsInRoleAsync(user,"BaseAdmin").Result)
            {
                var users = service.GetAll<User>().ToList();
                var model = users.Where(u => u.Id != user.Id && !userManager.IsInRoleAsync(u, "BaseAdmin").Result).Select(u => mapper.Map<UserViewModel>(u)).ToList();
                return View("BaseAdminMainPage",model);
            }
            if (userManager.IsInRoleAsync(user, "PROFESSIONADMIN").Result)
            {
                var outcomes = service.GetAll<FinalOutCome>().ToList();
                var model = outcomes.Where(o => user.Professions.Select(p => p.Id).Contains(o.ProfessionId.Value)).Select(o => mapper.Map<OutcomeViewModel>(o));
                return View("GraphPage",model);
            }   
            return NotFound();
        }

        public IActionResult BuildGraph(int professionId)
        {
            var outcomes = service.GetAll<FinalOutCome>().Where(o => o.ProfessionId == professionId).ToList();
            return View("GraphPage", outcomes);
        }

        [HttpGet]
        public IActionResult Edit(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Roles = roleManager.Roles.Select(r => mapper.Map<RoleViewModel>(r)).ToList();
            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            var model = mapper.Map<UserViewModel>(user);
            model.CurrentRoles = string.Join(",", userManager.GetRolesAsync(user).GetAwaiter().GetResult());
            return View("EditUser", model);
        }
    }
}