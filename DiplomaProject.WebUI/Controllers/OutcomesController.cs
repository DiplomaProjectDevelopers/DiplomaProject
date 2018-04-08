using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.Services;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DiplomaProject.WebUI.Controllers
{
    [Authorize(Roles = "ProfessionAdmin")]
    public class OutcomesController : BaseController
    {
        private OutcomesService outcomesService;
        public OutcomesController(IDPService service, OutcomesService outcomesService, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
                        : base(service, mapper, userManager, signInManager, roleManager)
        {
            this.outcomesService = outcomesService;
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
            ViewBag.Profession = new ProfessionViewModel
            {
                Id = professionId,
                Name = service.GetById<Profession>(professionId).Name
            };
            var model = outcomes.Select(o => mapper.Map<OutcomeViewModel>(o)).ToList();
            ViewBag.Outcomes = model;
            var edges = service.GetAll<Edge>().Where(e => e.ProfessionId == professionId).ToList();
            var viewModel = edges.Select(e => mapper.Map<EdgeViewModel>(e));
            return View("GraphView", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDependencies([FromBody]List<EdgeViewModel> model)
        {
            if (model != null)
            {
                var data = model.Select(edge => mapper.Map<Edge>(edge)).ToList();
                var processedData = await outcomesService.SaveDependencies(data);
                var processedModel = processedData.Select(d => mapper.Map<EdgeViewModel>(d));
                return Json(model, new JsonSerializerSettings {
                    ContractResolver = new DefaultContractResolver()
                });
            }
            return Json("Error");
        }
    }
}