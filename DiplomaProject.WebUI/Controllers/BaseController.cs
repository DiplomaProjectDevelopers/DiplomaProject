
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Extentions;
using DiplomaProject.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiplomaProject.WebUI.Controllers
{ 
    public class BaseController : Controller
    {
        protected IDPService service;
        protected IMapper mapper;
        protected UserManager<User> userManager;
        protected SignInManager<User> signInManager;
        protected RoleManager<Role> roleManager;
        protected IEmailSender emailSender;
        protected User currentUser;
         
        public BaseController(IDPService service, IMapper mapper,
            UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IEmailSender emailSender)
        {
            this.service = service;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected void GetRoles(params int[] professions)
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = service.GetUserAsync(User).Result;
                if (user != null)
                {
                    this.currentUser = user;
                    IEnumerable<string> userroles;
                    if (professions.Length > 0)
                    {
                        userroles = service.GetAll<UserRole>().Where(ur => professions.Contains(ur.ProfessionId) && ur.UserId == user.Id).
                                Select(s => s.RoleId).Select(s => roleManager.FindByIdAsync(s).Result.Name.ToUpper());
                    }
                    else
                    {
                        userroles = service.GetAll<UserRole>().Where(ur => ur.UserId == user.Id).
                                Select(s => s.RoleId).Select(s => roleManager.FindByIdAsync(s).Result.Name.ToUpper());
                    }
                    ViewBag.UserRoles = userroles.ToList();
                }
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = service.GetUserAsync(User).Result;
                if (user != null)
                {
                    this.currentUser = user;
                }
            }
            base.OnActionExecuting(context);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                service.Dispose();

            }
            base.Dispose(disposing);
        }

    }
}