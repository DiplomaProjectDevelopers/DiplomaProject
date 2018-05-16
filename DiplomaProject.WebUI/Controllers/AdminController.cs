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
        public AdminController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IEmailSender emailSender)
            : base(service, mapper, userManager, signInManager, roleManager, emailSender)
        {

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = userManager.GetUserAsync(User).Result;
            var users = service.GetAll<User>().ToList();
            var model = users.Where(u => u.Id != user.Id).
                Select(u =>  mapper.Map<UserViewModel>(u)    
                ).ToList();
            var um = mapper.Map<UserViewModel>(user);
            var userRoles = service.GetAll<UserRole>().ToList().Where(p => p.UserId == user.Id).Select(p => mapper.Map<UserRoleViewModel>(p)).ToList();
            for (int i = 0; i < userRoles.Count; i++)
            {
                userRoles[i].ProfessionName = service.GetById<Profession>(userRoles[i].ProfessionId)?.Name;
                userRoles[i].UserName = (await userManager.FindByIdAsync(userRoles[i].UserId))?.UserName;
                userRoles[i].RoleName = (await roleManager.FindByIdAsync(userRoles[i].RoleId))?.Name;
                userRoles[i].RoleDisplayName = (await roleManager.FindByIdAsync(userRoles[i].RoleDisplayName))?.DisplayName;
            }
            um.UserRoles = userRoles;

            um.Professions = service.GetAll<UserRole>().Where(up => up.UserId == user.Id).Select(p => p.ProfessionId).Distinct()
                .Select(s => mapper.Map<ProfessionViewModel>(service.GetById<Profession>(s))).ToList();

            ViewBag.User = um;
            return View("UserList", model);
        }

        public IActionResult UserList(string searchTerm)
        {
            GetRoles();
            var user = userManager.GetUserAsync(User).Result;
            var users = service.GetAll<User>().ToList();
            var model = users.Where(u => u.Id != user.Id).
                Select(u => mapper.Map<UserViewModel>(u)).ToList();
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
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var userroles = service.GetAll<UserRole>().Where(u => u.UserId == user.Id).ToList();
                    if (userroles.Count == 0)
                    {
                        ModelState.AddModelError("", "Դուք դեռ չունեք որևէ իրավասություն համակարգում: Դիմեք համակարգի ադմինիստրատորին դերերի բաշխման համար:");
                        return View(model);
                    }                    
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Չափից ավելի անհաջող մուտքի փորձ կատարվեց: Խնդրում ենք փորձել ավելի ուշ");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Սխալ մուտքանուն կամ գաղտնաբառ: Խնդրում ենք փորձել կրկին");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Մուտքանունի կամ գաղտնաբառի սխալ ձևաչափ");
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
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Admin", new { userId = user.Id, code }, protocol: Request.Scheme);
                    await emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
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
        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var current = await service.GetUserAsync(User);
            var da = (await roleManager.FindByNameAsync("DepartmentAdmin"));
            var dap = service.GetAll<UserRole>().Where(ur => ur.UserId == current.Id && ur.RoleId == da.Id)
                .Select(ur => ur.ProfessionId).ToList();
            if (dap.Count > 0)
            {
                ViewBag.Roles = service.GetAll<Role>().Where(r => r.Priority > 3).Select(s => mapper.Map<RoleViewModel>(s)).ToList();
                ViewBag.Professions = service.GetAll<Profession>().Where(p => dap.IndexOf(p.Id) != -1).Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
                var userroles = service.GetAll<UserRole>().Where(u => u.UserId == userId && dap.Contains(u.ProfessionId)).Select(s => mapper.Map<UserRoleViewModel>(s)).ToList();
                var model = mapper.Map<UserViewModel>(user);
                model.UserRoles = userroles;
                for (int i = 0; i < model.UserRoles.Count; i++)
                {
                    model.UserRoles[i].Id = -1 - i;
                }
                return View("EditUser", model);
            }
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
        public async Task<IActionResult> EditConfirmed([FromBody]UserViewModel model)
        {
            if (ModelState.IsValid && model != null && model.UserRoles.Count  != 0)
            {
                var currentUserProfessions = service.GetAll<UserRole>().Where(s => s.UserId == currentUser.Id).Select(s => s.ProfessionId).Distinct().ToList();
                var enabledRoles = service.GetAll<Role>().ToList();
                var previousroles = service.GetAll<UserRole>().Where(s => s.UserId == model.Id);
                foreach (var item in previousroles)
                {
                    await service.Delete(item);
                }
                for (int i = 0; i < model.UserRoles.Count; i++)
                {
                    if (currentUserProfessions.Contains(model.UserRoles[i].ProfessionId))
                    {
                        var dataModel = new UserRole
                        {
                            UserId = model.Id,
                            RoleId = model.UserRoles[i].RoleId,
                            ProfessionId = model.UserRoles[i].ProfessionId
                        };
                        await service.Insert(dataModel);
                    }
                }
                TempData["UserUpdated"] = Messages.USER_UPDATED_SUCCESS;
                return Json(new { redirect = Url.Action("Index", "Admin") });
            }
            ViewBag.Message = "Սխալ տեղի ունեցավ: Խնդրում ենք փորձել կրկին";
            return View("EditUser", model);
        }

        [HttpGet]
        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
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
        [Authorize(Roles = "BaseAdmin, DepartmentAdmin")]
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


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Գաղտնաբառը վերականգնելու համար սեղմեք հղման վրա <a href='{callbackUrl}'>այստեղ</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}