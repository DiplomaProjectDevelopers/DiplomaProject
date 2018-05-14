using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiplomaProject.WebUI.Controllers
{
    public class StatusCodeController : BaseController
    {
        private readonly ILogger<HomeController> logger;

        public StatusCodeController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, ILogger<HomeController> logger, IEmailSender emailSender)
            : base(service, mapper, userManager, signInManager, roleManager, emailSender)
        {
            this.logger = logger;
        }
        [HttpGet("/StatusCode/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            var reExecute = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            logger.LogInformation($"Unexpected Status Code: {statusCode}, OriginalPath: {reExecute?.OriginalPath}");
            return View(statusCode);
        }
    }
}