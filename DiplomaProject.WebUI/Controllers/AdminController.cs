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

namespace DiplomaProject.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IDPService service;
        private IMapper mapper;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        public AdminController(IDPService service, IMapper mapper,
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.service = service;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            var userId = userManager.GetUserId(HttpContext.User);
            var roles = (from r in await service.GetAll<UserRole>()
                         join role in await service.GetAll<Role>()
                         on r.RoleId equals role.Id
                         select role).ToList();
            if (roles.Find(r => r.Name == "BaseAdminMainPage") != null)
            {
                var users = (from user in await service.GetAll<User>()
                             join userrole in await service.GetAll<UserRole>() on user.Id equals userrole.UserId
                             join role in await service.GetAll<Role>() on userrole.RoleId equals role.Id
                             where role.Name != "BaseAdministrator"
                             select user).ToList();
                return View("BaseAdminPage", mapper.Map<IEnumerable<UserViewModel>>(users));
            }
            //switch (user.Role?.Name)
            //{
            //    case "ProfessionAdmin":
            //        var model = mapper.Map<ProfessionViewModel>(user);
            //        return View("ProfessionAdminMainPage", model);
            //    case "BaseAdmin":
            //        return View("BaseAdminMainPage");
            //}
            return NotFound();
            //var user = mapper.Map<ProfessionAdminViewModel>(
            //    await service.GetUserAsync(HttpContext.User));
            //return View(user);
        }

        public async Task<IActionResult> BuildGraph(int professionId)
        {
            var outcomes = (await service.GetAll<FinalOutCome>()).Where(o => o.ProfessionId == professionId).ToList();
            return View("GraphPage", outcomes);
        }
    }
}