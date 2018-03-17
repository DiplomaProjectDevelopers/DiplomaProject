using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Helpers;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.WebUI.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        public AdminController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        public IActionResult Index()
        {
            return Router();
        }

        [NonAction]
        public IActionResult Router()
        {
            var user = userManager.GetUserAsync(User).Result;
            if (userManager.IsInRoleAsync(user, "BaseAdmin").Result)
            {
                return RedirectToAction("GetUsers");
            }
            if (userManager.IsInRoleAsync(user, "PROFESSIONADMIN").Result)
            {
                var outcomes = service.GetAll<FinalOutCome>().ToList();
                var model = outcomes.Where(o => user.Professions.Select(p => p.Id).Contains(o.ProfessionId.Value)).Select(o => mapper.Map<OutcomeViewModel>(o));
                return View("GraphPage", model);
            }
            return NotFound();
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var user = userManager.GetUserAsync(User).Result;
            var users = service.GetAll<User>().ToList();
            var model = users.Where(u => u.Id != user.Id && !userManager.IsInRoleAsync(u, "BaseAdmin").Result).Select(u => mapper.Map<UserViewModel>(u)).ToList();
            return View("UserList", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Router();
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Router();
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = mapper.Map<User>(model);
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = "User was registered successfully!";
                    return RedirectToAction("Login");
                }
                else
                {
                    AddErrors(result);
                }
            }
            return View(model);
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
            model.CurrentRoles = userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Delete(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            var model = mapper.Map<UserViewModel>(user);
            model.CurrentRoles = userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            if (userId != null)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["userDeleted"] = Messages.USER_DELETED_SUCCESSFULLY;
                        return RedirectToAction(nameof(GetUsers));
                    }
                    else
                    {
                        var model = mapper.Map<UserViewModel>(user);
                        return View(model);
                    }
                }
            }
            return RedirectToAction("GetUsers");
        }

        [HttpGet]
        public IActionResult UpdateAccount()
        {
            var user = userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var model = mapper.Map<UpdateAccountViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(UpdateAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.GetUserAsync(User);
            var passwordChecked = await userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!passwordChecked)
            {
                ModelState.AddModelError("", "Current password field was wrong. Please try again.");
                return View(model);
            }

            if (model.Email != null && model.Email != user.Email)
            {
                await userManager.SetEmailAsync(user, model.Email);
            }

            if (model.PhoneNumber != null && model.PhoneNumber != user.PhoneNumber)
            {
                await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            }

            if (model.NewPassword != null && model.NewPasswordConfirm != null && model.NewPassword == model.NewPasswordConfirm)
            {
                await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            }
            return Router();
        }

    }
}