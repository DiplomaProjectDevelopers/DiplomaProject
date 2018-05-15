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
using System.Collections.Generic;

namespace DiplomaProject.WebUI.Controllers
{
    [Authorize]
    public class SubjectListController : BaseController
    {
        public SubjectListController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IEmailSender emailSender)
            : base(service, mapper, userManager, signInManager, roleManager, emailSender)
        {

        }
        [Authorize]
        [HttpGet]
        public IActionResult SubOut(int professionsId)
        {
            ViewBag.p = professionsId;
            GetRoles(professions: professionsId);
            if (professionsId == 0) return NotFound();
            var profession = service.GetById<Profession>(professionsId);
            ViewBag.ProfessionsList = mapper.Map<ProfessionViewModel>(profession);
            var modules = service.GetAll<SubjectModule>().Select(s => mapper.Map<SubjectModuleViewModel>(s)).ToList();
            var subjects = service.GetAll<Subject>().Where(s => s.ProfessionId == professionsId).Select(s => mapper.Map<SubjectViewModel>(s)).ToList();
            //for (int i = 0; i < subjects.Count; i++)
            //{
            //    modules.Find(m => m.Id == subjects[i].SubjectModuleId).Subjects.Add(subjects[i]);
            //}
            ViewBag.Subject = subjects;
            return View("SubjectListTable", modules);

        }
    }
}
//        public IActionResult SubjectList(string searchTerm)
//        {
//            GetRoles();
//            var user = userManager.GetUserAsync(User).Result;
//            var users = service.GetAll<Subject>().ToList();
//            var model = users.Where(u => (Convert.ToString(u.Id)) != user.Id).
//                Select(u => mapper.Map<SubjectViewModel>(u)).ToList();
//            //if (!string.IsNullOrEmpty(searchTerm))
//            //{
//            //    model = model.Where(u => u.Username.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1 || u.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1
//            //    || u.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1).ToList();
//            //}
//            return View("SubjectListTable", model);
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Login()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("Index");
//            }
//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await service.SignInAsync(model);
//                if (result.Succeeded)
//                {
//                    return RedirectToAction("Index", "Admin");
//                }
//                else
//                {
//                    ModelState.AddModelError("", "Wrong username or password. Please try again.");
//                }
//            }
//            else
//            {
//                ModelState.AddModelError("", "Incorrect username or password");
//            }
//            return View(model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize]
//        public async Task<IActionResult> LogOut()
//        {
//            await service.SignOutAsync();
//            return RedirectToAction(nameof(Index), "Home");
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Register()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("Index");
//            }
//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Register(RegisterViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = mapper.Map<User>(model);
//                var result = await userManager.CreateAsync(user, model.Password);
//                if (result.Succeeded)
//                {
//                    TempData["UserRegistered"] = Messages.USER_ADDED_SUCCESS;
//                    return RedirectToAction("Login");
//                }
//                else
//                {
//                    AddErrors(result);
//                }
//            }
//            return View(model);
//        }

//        [HttpGet]
//        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
//        public async Task<IActionResult> Edit(string userId)
//        {
//            if (userId is null)
//            {
//                return RedirectToAction("Index");
//            }
//            var user = await userManager.FindByIdAsync(userId);
//            if (user == null)
//                return NotFound();

//            var current = await service.GetUserAsync(User);
//            var da = (await roleManager.FindByNameAsync("DepartmentAdmin"));
//            var dap = service.GetAll<UserRole>().Where(ur => ur.UserId == current.Id && ur.RoleId == da.Id)
//                .Select(ur => ur.ProfessionId).ToList();
//            if (dap.Count > 0)
//            {
//                ViewBag.Roles = service.GetAll<Role>().Where(r => r.Priority > 3).Select(s => mapper.Map<RoleViewModel>(s)).ToList();
//                ViewBag.Professions = service.GetAll<Profession>().Where(p => dap.IndexOf(p.Id) != -1).Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
//                var userroles = service.GetAll<UserRole>().Where(u => u.UserId == userId && dap.Contains(u.ProfessionId)).Select(s => mapper.Map<UserRoleViewModel>(s)).ToList();
//                var model = mapper.Map<UserViewModel>(user);
//                model.UserRoles = userroles;
//                for (int i = 0; i < model.UserRoles.Count; i++)
//                {
//                    model.UserRoles[i].Id = -1 - i;
//                }
//                return View("EditUser", model);
//            }
//            throw new NotImplementedException();
//        }

//        [HttpPost]
//        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
//        public async Task<IActionResult> EditConfirmed([FromBody]UserViewModel model)
//        {
//            if (ModelState.IsValid && model != null && model.UserRoles.Count != 0)
//            {
//                var currentUserProfessions = service.GetAll<UserRole>().Where(s => s.UserId == currentUser.Id).Select(s => s.ProfessionId).Distinct().ToList();
//                var enabledRoles = service.GetAll<Role>().ToList();
//                var previousroles = service.GetAll<UserRole>().Where(s => s.UserId == model.Id);
//                foreach (var item in previousroles)
//                {
//                    await service.Delete(item);
//                }
//                for (int i = 0; i < model.UserRoles.Count; i++)
//                {
//                    if (currentUserProfessions.Contains(model.UserRoles[i].ProfessionId))
//                    {
//                        var dataModel = new UserRole
//                        {
//                            UserId = model.Id,
//                            RoleId = model.UserRoles[i].RoleId,
//                            ProfessionId = model.UserRoles[i].ProfessionId
//                        };
//                        await service.Insert(dataModel);
//                    }
//                }
//                TempData["UserUpdated"] = Messages.USER_UPDATED_SUCCESS;
//                return Json(new { redirect = Url.Action("Index", "Admin") });
//            }
//            ViewBag.Message = "Սխալ տեղի ունեցավ: Խնդրում ենք փորձել կրկին";
//            return View("EditUser", model);
//        }

//        [HttpGet]
//        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
//        public IActionResult Delete(string userId)
//        {
//            if (userId is null)
//            {
//                return RedirectToAction("Index");
//            }
//            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
//            var model = mapper.Map<UserViewModel>(user);
//            model.CurrentRole = userManager.GetRoleAsync(user).GetAwaiter().GetResult();
//            return View(model);
//        }

//        [HttpPost, ActionName("Delete")]
//        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(string userId)
//        {
//            if (userId != null)
//            {
//                var user = await userManager.FindByIdAsync(userId);
//                if (user != null)
//                {
//                    var result = await userManager.DeleteAsync(user);
//                    if (result.Succeeded)
//                    {
//                        TempData["UserDeleted"] = Messages.USER_DELETED_SUCCESS;
//                        return RedirectToAction(nameof(Index));
//                    }
//                    else
//                    {
//                        var model = mapper.Map<UserViewModel>(user);
//                        return View(model);
//                    }
//                }
//            }
//            return RedirectToAction("Users");
//        }

//        [HttpGet]
//        public IActionResult UpdateAccount()
//        {
//            var user = userManager.GetUserAsync(User).GetAwaiter().GetResult();
//            var model = mapper.Map<UpdateAccountViewModel>(user);
//            return View(model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UpdateAccount(UpdateAccountViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            var user = await userManager.GetUserAsync(User);
//            var passwordChecked = await userManager.CheckPasswordAsync(user, model.CurrentPassword);
//            if (!passwordChecked)
//            {
//                ModelState.AddModelError("", "Current password field was wrong. Please try again.");
//                return View(model);
//            }

//            if (model.Email != null && model.Email != user.Email)
//            {
//                await userManager.SetEmailAsync(user, model.Email);
//            }

//            if (model.PhoneNumber != null && model.PhoneNumber != user.PhoneNumber)
//            {
//                await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
//            }

//            if (model.NewPassword != null && model.NewPasswordConfirm != null && model.NewPassword == model.NewPasswordConfirm)
//            {
//                await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
//            }
//            TempData["AccountUpdated"] = Messages.ACCOUNT_UPDATED_SUCCESS;
//            return RedirectToAction("Index");
//        }
//        //[NonAction]
//        //private IActionResult Router()
//        //{
//        //    var user = userManager.GetUserAsync(User).Result;
//        //    var roles = roleManager.Roles.Where(r => userManager.GetRolesAsync(user).Result.Contains(r.Name));
//        //    var primaryRole = roles.OrderBy(r => r.Priority).First();
//        //    switch (primaryRole?.Name?.ToLower())
//        //    {
//        //        case "baseadmin":
//        //            return RedirectToAction("GetUsers");
//        //        case "professionadmin":
//        //            return RedirectToAction(nameof(OutcomesController.Index), "Outcomes");
//        //        case "defaultrole":
//        //            var model = mapper.Map<UserViewModel>(user);
//        //            return View("DefaultRoleUserPage", model);
//        //        default:
//        //            return NotFound();
//        //    }
//        //}

//        ////[HttpGet("Subject")]
//        ////[Authorize(Roles = "BaseAdmin")]
//        //public IActionResult GetSubjects()
//        //{
//        //    var a = service.GetById<Subject>(3);

//        //    var sub = userManager.GetUserAsync(User).Result;
//        //    var subs = service.GetAll<Subject>().ToList();
//        //    // ViewBag.Roles = roleManager.Roles.Select(r => mapper.Map<SubjectViewModel>(r)).ToList();
//        //    var mod = subs.Where(s => Convert.ToString(s.Id) != sub.Id).
//        //        Select(async u =>
//        //        {
//        //            var m = mapper.Map<SubjectListViewModel>(u);
//        //            /*    var roleName = await userManager.GetRoleAsync(u);
//        //                if (roleName != null)
//        //                {
//        //                    m.ProfessionId = (await roleManager.FindByNameAsync(subs.Name).Id;
//        //                }*/
//        //            return m;
//        //        }
//        //        ).Select(u => u.Result).ToList();
//        //    //~/Views/Subject/ .cshtml
//        //    return View("SubjectListTable", mod);
//        //}
//    }
//}
