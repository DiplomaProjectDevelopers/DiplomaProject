
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public abstract class BaseController : Controller
    {
        protected IDPService service;
        protected IMapper mapper;
        protected UserManager<User> userManager;
        protected SignInManager<User> signInManager;
        protected RoleManager<Role> roleManager;
        protected string roleName;

        public BaseController(IDPService service, IMapper mapper,
            UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            this.service = service;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            SetRoleName();
        }

        private void SetRoleName()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = service.GetUserAsync(User).Result;
                if (user != null)
                {
                    this.roleName = userManager.GetRoleAsync(user).Result;
                }
            }
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.RoleName = roleName?.ToUpper();
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