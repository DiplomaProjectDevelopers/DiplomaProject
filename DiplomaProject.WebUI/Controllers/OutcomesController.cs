using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject.WebUI.Controllers
{
    [Authorize(Roles = "ProfessionAdmin")]
    public class OutcomesController : BaseController
    {
        public OutcomesController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
                        : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = userManager.GetUserAsync(User).Result;
            //var outcomes = service.GetAll<FinalOutCome>().ToList();
            var professions = service.GetAll<Profession>().Where(e => e.AdminId == user.Id).ToList();
            var model = professions.Select(p =>
            {
                var profession = mapper.Map<ProfessionViewModel>(p);
                profession.Department = service.GetById<Department>(profession.DepartmentId.Value).Name;
                return profession;
            });
            //var model = outcomes.Where(o => user.Professions.Select(p => p.Id).Contains(o.ProfessionId.Value)).Select(o => mapper.Map<OutcomeViewModel>(o)).ToList();
            return View("ProfessionList", model);
        }

        [HttpGet]
        public IActionResult BuildGraph(int professionId)
        {
            var outcomes = service.GetAll<FinalOutCome>().Where(o => o.ProfessionId == professionId).ToList();
            ViewBag.ProfessionName = service.GetById<Profession>(professionId).Name;
            var model = outcomes.Select(o => mapper.Map<OutcomeViewModel>(o)).ToList();
            return View("GraphView", model);
        }

        [HttpPost]
        public async Task GetOutcomeDepemdencies(List<EdgeViewModel> model)
        {

        }
    }
}