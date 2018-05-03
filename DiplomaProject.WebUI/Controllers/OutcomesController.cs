using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.Models;
using DiplomaProject.Domain.Services;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DiplomaProject.WebUI.Controllers
{
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
            var professions = service.GetAll<Profession>().ToList();//.Where(e => e.AdminId == user.Id).ToList();
            var model = professions.Select(p =>
            {
                var profession = mapper.Map<ProfessionViewModel>(p);
                profession.Department = service.GetById<Department>(profession.DepartmentId.Value).Name;
                return profession;
            });
            return View("ProfessionList", model);
        }

        [HttpGet]
        [Authorize(Roles = "SubjectMaker")]
        public IActionResult MakeDependencies(int professionId)
        {
            GetRoles(professionId);
            var outcomes = service.GetAll<FinalOutCome>().Where(o => o.ProfessionId == professionId).ToList();
            var model = outcomes.Select(o => mapper.Map<OutcomeViewModel>(o)).ToList();
            foreach (var o in model)
            {
                if (o.SubjectId.HasValue && o.SubjectId.Value != 0)
                {
                    o.Subject = service.GetById<InitialSubject>(o.SubjectId.Value)?.Name;
                }
            }
            ViewBag.Outcomes = model;

            ViewBag.Profession = new ProfessionViewModel
            {
                Id = professionId,
                Name = service.GetById<Profession>(professionId).Name
            };

            var edges = service.GetAll<Edge>().Where(e => e.ProfessionId == professionId).ToList();
            var viewModel = edges.Select(e => mapper.Map<EdgeViewModel>(e));
            return View("DependencyBuilder", viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GraphViewer(int professionId)
        {
            GetRoles(professionId);
            ViewBag.Profession = new ProfessionViewModel
            {
                Id = professionId,
                Name = service.GetById<Profession>(professionId).Name
            };

            var outcomes = service.GetAll<FinalOutCome>().Where(o => o.ProfessionId == professionId).ToList();
            var model = outcomes.Select(o => mapper.Map<OutcomeViewModel>(o)).ToList();

            foreach (var o in model)
            {
                if (o.SubjectId.HasValue && o.SubjectId.Value != 0)
                {
                    o.Subject = service.GetById<InitialSubject>(o.SubjectId.Value)?.Name;
                }
            }
            ViewBag.Outcomes = model;

            var edges = service.GetAll<Edge>().Where(e => e.ProfessionId == professionId).ToList();
            var viewModel = edges.Select(e => mapper.Map<EdgeViewModel>(e));
            return View("GraphViewer", viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "SubjectMaker")]
        public async Task<IActionResult> SaveDependencies([FromBody]List<EdgeViewModel> model)
        {
            if (model != null)
            {
                var data = model.Select(edge => mapper.Map<Edge>(edge)).ToList();
                var processedData = await outcomesService.SaveDependencies(data);
                var processedModel = processedData.Select(d => mapper.Map<EdgeViewModel>(d));
                return Json(model, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                });
            }
            return Json("Error");
        }

        [HttpGet, ActionName("Subjects")]
        [Authorize(Roles = "SubjectMaker")]
        public IActionResult BuildSubgraphs(int? professionId)
        {
            if (!professionId.HasValue || professionId.Value <= 0)
                return NotFound();
            GetRoles(professionId.Value);
            var subjectList = new SubjectListViewModel
            {
                Profession = mapper.Map<ProfessionViewModel>(service.GetById<Profession>(professionId.Value))
            };
            ViewBag.Modules = service.GetAll<SubjectModule>().Select(m => mapper.Map<SubjectModuleViewModel>(m)).ToList();
            if (service.GetAll<FinalOutCome>().Where(o => o.ProfessionId == professionId).All(o => o.SubjectId.HasValue && o.SubjectId.Value > 0))
            {

                subjectList.Subjects = service.GetAll<Subject>().Where(s => s.ProfessionId == professionId).Select(s => mapper.Map<SubjectViewModel>(s)).ToList();
                foreach (var subject in subjectList.Subjects)
                {
                    var outcomes = service.GetAll<FinalOutCome>().Where(o => o.SubjectId == subject.Id).Select(o => mapper.Map<OutcomeViewModel>(o)).ToList();
                    subject.Outcomes = outcomes;
                }
            }
            else
            {
                var vertices = service.GetAll<FinalOutCome>().Where(o => o.ProfessionId == professionId).Select(o => o.Id).ToList();
                var edges = service.GetAll<Edge>().Where(e => e.ProfessionId == professionId).ToList();
                var graph = new Graph(vertices.Count);
                foreach (var edge in edges)
                {
                    var index1 = vertices.FindIndex(v => edge.LeftOutComeId.Value == v);
                    var index2 = vertices.FindIndex(v => edge.RightOutComeId.Value == v);
                    graph.AddEdge(index1, index2);
                }

                var subgraphs = graph.GetSubgraphs();
                for (int i = 0; i < subgraphs.Count; i++)
                {
                    var subject = new SubjectViewModel
                    {
                        Id = -(i + 1),
                        Name = $"Առարկա-{i}",
                        ProfessionId = professionId
                    };
                    for (int j = 0; j < subgraphs[i].Count; j++)
                    {
                        var outcome = service.GetById<FinalOutCome>(vertices[subgraphs[i][j]]);
                        var model = mapper.Map<OutcomeViewModel>(outcome);
                        subject.Outcomes.Add(model);
                    }
                    subjectList.Subjects.Add(subject);
                }
            }
            return View("SubjectListPreview", subjectList);
        }

        [HttpPost]
        public IActionResult Proff(ProfessionViewModel profession_select)
        {
            if (profession_select.Id == 0) { }
            string selectedName = profession_select.Name;
            ViewBag.SelectName = selectedName;
            var profession = service.GetAll<Profession>().ToList();
            var model = profession.Select(p => mapper.Map<ProfessionViewModel>(p));
            List<ProfessionViewModel> professionList = model.Select(p => new ProfessionViewModel { Name = p.Name }).ToList();
            professionList.Insert(0, new ProfessionViewModel { Id = 0, Name = "մասնագիտություն" });
            ViewBag.ListOfProfession = professionList;
            ProfessionViewModel selectedProfession = professionList.Find(p => p.Name == selectedName);
            int selectedId = selectedProfession.Id;
            TempData["SelectedId"] = selectedId;
            return View("Questionnaire1", model);
        }

        [HttpPost]
        public IActionResult StakeHolder()
        {
            var stakeHolders = service.GetAll<StakeHolder>().ToList();
            var model = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s));
            List<StakeHolderViewModel> stakeHoldersList = model.Select(s => new StakeHolderViewModel { CompanyName = s.CompanyName }).ToList();
            stakeHoldersList.Insert(0, new StakeHolderViewModel { Id = 0, CompanyName = "կազմակերպություն" });
            ViewBag.ListofStakeHolder = stakeHoldersList;
            return View("Questionnaire1", model);
        }

        [HttpPost]
        public IActionResult StakeHolderType()
        {
            var stakeHolderType = service.GetAll<StakeHolderType>().ToList();
            var model = stakeHolderType.Select(st => mapper.Map<StakeHolderTypeViewModel>(st));
            List<StakeHolderTypeViewModel> stakeHolderTypeList = model.Select(st => new StakeHolderTypeViewModel { Id = st.Id, TypeName = st.TypeName }).ToList();
            ViewBag.ListOfStakeHolderType = stakeHolderTypeList;
            return View("Questionnaire1", model);
        }

        [HttpPost]
        public IActionResult Subject()
        {
            var subject = service.GetAll<Subject>().ToList();
            var model = subject.Select(sb => mapper.Map<SubjectViewModel>(sb));
            List<SubjectViewModel> subjectlist = model.Select(sb => new SubjectViewModel { Id = sb.Id, Name = sb.Name, ProfessionId = sb.ProfessionId }).ToList();
            ViewBag.ListofSubject = subjectlist;
            return View();
        }
    }
}