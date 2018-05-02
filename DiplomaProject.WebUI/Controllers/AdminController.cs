using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var user = userManager.GetUserAsync(User).Result;
            var users = service.GetAll<User>().ToList();
            var model = users.Where(u => u.Id != user.Id).
                Select(async u =>
                {
                    var m = mapper.Map<UserViewModel>(u);
                    var roleName = await userManager.GetRoleAsync(u);
                    var r = await roleManager.FindByNameAsync(roleName);
                    m.SelectedRoleId = r.Id;
                    return m;
                }
                ).Select(u => u.Result).ToList();
            var um = mapper.Map<UserViewModel>(user);
            var proff = service.GetAll<UserRole>().ToList().Where(p => p.UserId == user.Id && p.ProfessionId > 0).Select(ur => ur.ProfessionId).Distinct()
                .Select(p => service.GetById<Profession>(p)).Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            ViewBag.User = um;
            return View("UserList", model);
        }

        public IActionResult UserList(string searchTerm)
        {
            var user = userManager.GetUserAsync(User).Result;
            var users = service.GetAll<User>().ToList();
            var model = users.Where(u => u.Id != user.Id).
                Select(async u =>
                {
                    var m = mapper.Map<UserViewModel>(u);
                    var roleName = await userManager.GetRoleAsync(u);
                    var r = await roleManager.FindByNameAsync(roleName);
                    m.SelectedRoleId = r.Id;
                    return m;
                }
                ).Select(u => u.Result).ToList();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(u => u.Username.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1 || u.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1
                || u.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
            return PartialView("_Users", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
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
        [Authorize]
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
                return RedirectToAction("Index");
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
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "DefaultRole");
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
        [Authorize(Roles = "BaseAdmin, DepartmentAdmin, FacultyAdmin")]
        public async Task<IActionResult> UpdateRole([FromBody]UserRoleViewModel model)
        {
            var userId = model.UserId;
            var roleId = model.RoleId;
            var user = await userManager.FindByIdAsync(userId);
            var role = await roleManager.FindByIdAsync(roleId);
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
        [Authorize(Roles = "BaseAdmin, FacultyAdmin, DepartmentAdmin")]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            if (user == null)
                return NotFound();

            var current = service.GetUserAsync(User).GetAwaiter().GetResult();
            var da = (await roleManager.FindByNameAsync("DepartmentAdmin"));
            var dap = service.GetAll<UserRole>().Where(ur => ur.UserId == current.Id && ur.RoleId == da.Id)
                .Select(ur => ur.ProfessionId).ToList();
            if (dap.Count > 0)
            {
                //    .Where(u => u.UserId == current.Id && u.ProfessionId.HasValue && u.ProfessionId.Value > 0)
                //    .Select(s => s.ProfessionId.Value).Distinct().ToList();

                ViewBag.Roles = service.GetAll<Role>().Where(r => r.Priority > 3).Select(s => mapper.Map<RoleViewModel>(s)).ToList();
                ViewBag.Professions = service.GetAll<Profession>().Where(p => dap.IndexOf(p.Id) != -1).Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
                var userroles = service.GetAll<UserRole>().Where(u => u.UserId == userId && dap.Contains(u.ProfessionId)).Select(s => mapper.Map<UserRoleViewModel>(s)).ToList();
                var model = mapper.Map<UserViewModel>(user);
                model.UserRoles = userroles;
                for (int i = 0; i < model.UserRoles.Count; i++)
                {
                    model.UserRoles[i].Id = i + 1;
                }
                return View("EditProfessionAdmin", model);
            }
            throw new NotImplementedException();
        }

        [HttpPost, ActionName("EditProfessionAdmin")]
        [Authorize(Roles = "BaseAdmin, FacultyAdmin, DepartmentAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(ProfessionAdminViewModel model)
        {
            if (ModelState.IsValid && model != null && model.Id != null)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                var userProfessions = service.GetAll<Profession>();
                //if (userProfessions.FirstOrDefault(p => p.Id == model.ProfessionId) != null)
                //{
                //    var profession = service.GetById<Profession>(model.ProfessionId);
                //    profession.AdminId = user.Id;
                //    await service.Update<Profession>(profession);
                //}
                TempData["UserUpdated"] = Messages.USER_UPDATED_SUCCESS;
                return RedirectToAction("Users");
            }
            throw new Exception();
        }

        [HttpGet]
        [Authorize(Roles = "BaseAdmin, FacultyAdmin, DepartmentAdmin")]
        public IActionResult Delete(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            var model = mapper.Map<UserViewModel>(user);
            model.CurrentRole = userManager.GetRoleAsync(user).GetAwaiter().GetResult();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "BaseAdmin, FacultyAdmin, DepartmentAdmin")]
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
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var model = mapper.Map<UserViewModel>(user);
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Users");
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
            TempData["AccountUpdated"] = Messages.ACCOUNT_UPDATED_SUCCESS;
            return RedirectToAction("Index");
        }

    }
}