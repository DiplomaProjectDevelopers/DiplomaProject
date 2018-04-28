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
            if (user is null) return NotFound();
            var roles = roleManager.Roles.Where(r => userManager.GetRolesAsync(user).Result.Contains(r.Name));
            var primaryRole = roles.OrderBy(r => r.Priority).First();
            switch (primaryRole?.Name?.ToLower())
            {
                case "baseadmin":
                    return RedirectToAction("Users");
                case "professionadmin":
                    return RedirectToAction(nameof(OutcomesController.Index), "Outcomes");
                case "defaultrole":
                    var model = mapper.Map<UserViewModel>(user);
                    return View("DefaultRoleUserPage", model);
                default:
                    return NotFound();
            }
        }

        [HttpGet,ActionName("Users")]
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

        [HttpGet, ActionName("Stakeholders")]
        [Authorize(Roles = "BaseAdmin")]
        public IActionResult GetStakeholders()
        {
            var stakeholders = service.GetAll<StakeHolder>().Select(s => mapper.Map<StakeHolderViewModel>(s)).ToList();
            foreach (var s in stakeholders)
            {
                if (s.BranchId.HasValue && s.BranchId.Value > 0)
                {
                    s.BranchName = service.GetById<Branch>(s.BranchId.Value)?.Name;
                }
                if (s.TypeId.HasValue && s.TypeId.Value > 0)
                {
                    var type = service.GetById<StakeHolderType>(s.TypeId.Value);
                    s.TypeName = type.ProfessionName ?? type.TypeName;
                }
            }
            return View("StakaholderList", stakeholders);
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
        [Authorize(Roles = "BaseAdmin")]
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
        [Authorize(Roles = "BaseAdmin")]
        public IActionResult Edit(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            var role = userManager.GetRoleAsync(user).GetAwaiter().GetResult();
            switch (role.ToUpper())
            {
                case "PROFESSIONADMIN":
                    ViewBag.Professions = service.GetAll<Profession>().ToList();
                    var model = mapper.Map<ProfessionAdminViewModel>(user);
                    model.CurrentRole = role;
                    return View("EditProfessionAdmin", model);

                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost, ActionName("EditProfessionAdmin")]
        [Authorize(Roles = "BaseAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(ProfessionAdminViewModel model)
        {
            if (ModelState.IsValid && model != null && model.Id != null)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                var userProfessions = service.GetAll<Profession>();
                if (userProfessions.FirstOrDefault(p => p.Id == model.ProfessionId) != null)
                {
                    var profession = service.GetById<Profession>(model.ProfessionId);
                    profession.AdminId = user.Id;
                    await service.Update<Profession>(profession);
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
            model.CurrentRole = userManager.GetRoleAsync(user).GetAwaiter().GetResult();
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
            return Router();
        }

    }
}