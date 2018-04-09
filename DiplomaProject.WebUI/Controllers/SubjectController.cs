using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;
using DiplomaProject.Domain.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.WebUI.Controllers
{
    public class SubjectController : BaseController
    {
        public SubjectController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        [HttpPost]
        public IActionResult Index(SubjectViewModel profession)
        {
            var subject = service.GetAll<Subject>().ToList();
            var model = subject.Select(sb => mapper.Map<SubjectViewModel>(sb));
            List<SubjectViewModel> subjectlist = model.Select(sb => new SubjectViewModel { Id = sb.Id, Name = sb.Name, ProfessionId = sb.ProfessionId }).ToList();
            ViewBag.ListofSubject = subjectlist;
            return View();
        }
    }
}
