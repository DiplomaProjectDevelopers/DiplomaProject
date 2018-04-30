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

        [Authorize]
        public IActionResult Index()
        {
            var user = userManager.GetUserAsync(User).Result;
            if (user is null) return NotFound();
            return RedirectToAction("Users");
            //var primaryRole = userManager.GetRoleAsync(user).GetAwaiter().GetResult();
            //switch (primaryRole)
            //{
            //    case "facultyadmin":
            //        return View("Users");
            //    case "departmentadmin":
            //        return View();
            //    case "professionadmin":
            //        return RedirectToAction(nameof(OutcomesController.Index), "Outcomes");
            //    case "requestsender":
            //        return View();
            //    case "subjectmaker":
            //        return View();
            //    case "labormaker":
            //        return View();
            //    case "curriculummaker":
            //        return View();
            //    case "defaultrole":
            //        var model = mapper.Map<UserViewModel>(user);
            //        return View("DefaultRoleUserPage", model);
            //    default:
            //        return NotFound();
            //}
        }

        [NonAction]
        private IActionResult Router()
        {
            var user = userManager.GetUserAsync(User).Result;
            if (user is null) return NotFound();
            var primaryRole = userManager.GetRoleAsync(user).GetAwaiter().GetResult();
            switch (primaryRole)
            {
                case "facultyadmin":
                    return RedirectToAction("Users");
                case "departmentadmin":
                    return View();
                case "professionadmin":
                    return RedirectToAction(nameof(OutcomesController.Index), "Outcomes");
                case "requestsender":
                    return View();
                case "subjectmaker":
                    return View();
                case "labormaker":
                    return View();
                case "curriculummaker":
                    return View();
                case "defaultrole":
                    var model = mapper.Map<UserViewModel>(user);
                    return View("DefaultRoleUserPage", model);
                default:
                    return NotFound();
            }
        }

        [HttpGet, ActionName("Users")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var user = userManager.GetUserAsync(User).Result;
            var users = service.GetAll<User>().ToList();
            var role = await roleManager.FindByNameAsync(userManager.GetRoleAsync(user).GetAwaiter().GetResult());
            ViewBag.Roles = roleManager.Roles.Select(r => mapper.Map<RoleViewModel>(r)).ToList();
            var model = users.Where(u => u.Id != user.Id).
                Select(async u =>
                {
                    var m = mapper.Map<UserViewModel>(u);
                    var roleName = await userManager.GetRoleAsync(u);
                    var r = await roleManager.FindByNameAsync(roleName);
                    m.SelectedRoleId = r.Id;
                    m.CanEdit = r.Priority > role.Priority;
                    return m;
                }
                ).Select(u => u.Result).ToList();
            var um = mapper.Map<UserViewModel>(user);
            um.Professions = service.GetAll<Profession>().Where(p => p.AdminId == user.Id).Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            ViewBag.User = um;
            return View("UserList", model);
        }

        [HttpGet, ActionName("Stakeholders")]
        [Authorize]
        public async Task<IActionResult> GetStakeholders()
        {
            var user = await userManager.GetUserAsync(User);
            var role = await roleManager.FindByNameAsync(await userManager.GetRoleAsync(user));
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
                if (role.Priority <= 3) s.CanEdit = true;
            }
            var um = mapper.Map<UserViewModel>(user);
            um.Professions = service.GetAll<Profession>().Where(p => p.AdminId == user.Id).Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            ViewBag.User = um;
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
        public IActionResult Edit(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            var role = userManager.GetRoleAsync(user).GetAwaiter().GetResult();
            ViewBag.Professions = service.GetAll<Profession>().ToList();
            var model = mapper.Map<ProfessionAdminViewModel>(user);
            model.CurrentRole = role;
            return View("EditProfessionAdmin", model);
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
                if (userProfessions.FirstOrDefault(p => p.Id == model.ProfessionId) != null)
                {
                    var profession = service.GetById<Profession>(model.ProfessionId);
                    profession.AdminId = user.Id;
                    await service.Update<Profession>(profession);
                }
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