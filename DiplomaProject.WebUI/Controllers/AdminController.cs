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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (userManager.IsInRoleAsync(user,"BaseAdmin").Result)
            {
                var users = (await service.GetAll<User>()).Where(u => u.Id != user.Id && !userManager.IsInRoleAsync(u, "BaseAdmin").GetAwaiter().GetResult());
                return View("BaseAdminMainPage",mapper.Map<IEnumerable<UserViewModel>>(users));
            }
            if (userManager.IsInRoleAsync(user, "PROFESSIONADMIN").Result)
            {
                var outcomes = await service.GetAll<FinalOutCome>();
                var model = outcomes.Where(o => user.Professions.Select(p => p.Id).Contains(o.ProfessionId.Value)).Select(o => mapper.Map<OutcomeViewModel>(o));
                return View("GraphPage",model);
            }   
            return NotFound();
        }

        public async Task<IActionResult> BuildGraph(int professionId)
        {
            var outcomes = (await service.GetAll<FinalOutCome>()).Where(o => o.ProfessionId == professionId).ToList();
            return View("GraphPage", outcomes);
        }
    }
}