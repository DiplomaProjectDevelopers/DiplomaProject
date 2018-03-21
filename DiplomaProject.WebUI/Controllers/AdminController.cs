using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Extentions;
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
        private IActionResult Router()
        {
            var user = userManager.GetUserAsync(User).Result;
            if (userManager.IsInRoleAsync(user, "BaseAdmin").Result)
            {
                return RedirectToAction("GetUsers");
            }
            if (userManager.IsInRoleAsync(user, "PROFESSIONADMIN").Result)
            {
                return RedirectToAction(nameof(OutcomesController.Index), "Outcomes");
            }
            return NotFound();
        }

        [HttpGet("Users")]
        [Authorize(Roles = "BaseAdmin")]
        public IActionResult GetUsers()
        {
            var user = userManager.GetUserAsync(User).Result;
            var users = service.GetAll<User>().ToList();
            ViewBag.Roles = roleManager.Roles.Select(r => mapper.Map<RoleViewModel>(r)).ToList();
            var model = users.Where(u => u.Id != user.Id && !userManager.IsInRoleAsync(u, "BaseAdmin").Result).
                Select(async u =>
                {
                    var m = mapper.Map<UserViewModel>(u);
                    var roleName = await userManager.GetRoleAsync(u);
                    if (roleName != null)
                    {
                        m.SelectedRoleId = (await roleManager.FindByNameAsync(roleName)).Id;
                    }
                    return m;
                }
                ).Select(u => u.Result).ToList();
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
            return RedirectToAction(nameof(Index), "Home");
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
                    TempData["UserRegistered"] = Messages.USER_ADDED_SUCCESS;
                    return RedirectToAction("Login");
                }
                else
                {
                    AddErrors(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "BaseAdmin")]
        public async Task<IActionResult> UpdateRole([FromBody]UserRoleViewModel model)
        {
            var userId = model.UserId;
            var roleId = model.RoleId;
            var user = await userManager.FindByIdAsync(userId);
            var role = await roleManager.FindByIdAsync(roleId);
            if (roleManager.Roles.Count() == 2)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "TestRole",
                    NormalizedName = "TESTROLE",
                });
            }
            if (user != null && role != null)
            {
                if (await userManager.IsInRoleAsync(user, role.Name) == false)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, roles);
                    await userManager.AddToRoleAsync(user, role.Name);
                    return Json(new { Message = Messages.ROLE_UPDATED_SUCCESS, Type = "success" });
                }
            }
            return Json(new { Message = Messages.ROLE_UPDATED_FAILURE, Type = "danger" });
        }
        [HttpGet]
        [Authorize(Roles = "BaseAdmin")]
        public IActionResult Edit(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Roles = roleManager.Roles.Select(r => mapper.Map<RoleViewModel>(r)).ToList();
            ViewBag.Professions = service.GetAll<Profession>().ToList();
            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            var model = mapper.Map<UserViewModel>(user);
            model.CurrentRoles = userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "BaseAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(UserViewModel model)
        {
            if (ModelState.IsValid && model != null && model.Id != null && model.SelectedRoleId != null)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                var role = await roleManager.FindByIdAsync(model.SelectedRoleId);
                if (user != null && role != null && userManager.IsInRoleAsync(user, role.Name).GetAwaiter().GetResult() == false)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                }
                TempData["UserUpdated"] = Messages.USER_UPDATED_SUCCESS;
                return RedirectToAction(nameof(GetUsers));
            }
            throw new Exception();
        }

        [HttpGet]
        [Authorize(Roles = "BaseAdmin")]
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
        [Authorize(Roles = "BaseAdmin")]
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
                        TempData["UserDeleted"] = Messages.USER_DELETED_SUCCESS;
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
        [ValidateAntiForgeryToken]
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
            TempData["Accountpdated"] = Messages.ACCOUNT_UPDATED_SUCCESS;
            return Router();
        }

    }
}