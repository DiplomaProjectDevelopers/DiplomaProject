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
using DiplomaProject.Domain.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaProject.WebUI.Controllers
{
    public class SubjectController : BaseController
    {
        public SubjectController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(int professionId)
        {
            if (professionId == 0) return NotFound();
            var profession = service.GetById<Profession>(professionId);
            if (profession == null) return NotFound();
            var subjects = service.GetAll<Subject>().Where(s => s.ProfessionId == professionId).Select(s => mapper.Map<SubjectViewModel>(s)).ToList();
            for (int i = 0; i < subjects.Count; i++)
            {
                if (subjects[i].SubjectModuleId.HasValue && subjects[i].SubjectModuleId.Value != 0)
                {
                    subjects[i].SubjectModule = service.GetById<SubjectModule>(subjects[i].SubjectModuleId.Value)?.Name;
                }
            }
            ViewBag.Profession = mapper.Map<ProfessionViewModel>(profession);
            return View("SubjectList", subjects);
        }
            var subject = service.GetAll<Subject>().ToList();
            var sbmodel = subject.Select(sb => mapper.Map<SubjectViewModel>(sb)).ToList();
            var subjectlist = sbmodel.Select(sb => sb.Name).ToList();
            ViewBag.ListofSubject = subjectlist;
            var finaloutcome = service.GetAll<FinalOutCome>().ToList();
            var fomodel = finaloutcome.Select(fo => mapper.Map<FinalOutcomeViewModel>(fo));
            ViewBag.ListOfFinalOutcome = finaloutcome;
            return View("SubjectDistribution");
        }

        [HttpPost]
        public async Task<IActionResult> SaveSubjects([FromBody]SubjectListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Առարկայական մոդուլ և առարկայի անուն դաշտերը պարտադիր են");
            }
            for (int i = 0; i < model.Subjects.Count; ++i)
            {
                var outcomes = model.Subjects[i].Outcomes;
                var subject = mapper.Map<Subject>(model.Subjects[i]);
                if (subject.Id > 0)
                {
                    await service.Update(subject);
                }
                else
                {
                    subject.Id = 0;
                    await service.Insert(subject);
                    model.Subjects[i].Id = subject.Id;
                }
                model.Subjects[i].Outcomes = outcomes;
                List<FinalOutCome> list = new List<FinalOutCome>();
                for (int j = 0; j < model.Subjects[i].Outcomes.Count; ++j)
                {
                    var outcome = model.Subjects[i].Outcomes[j];
                    var dboutcome = service.GetById<FinalOutCome>(outcome.Id);
                    dboutcome.SubjectId = subject.Id;
                    list.Add(dboutcome);
                    model.Subjects[i].Outcomes[j] = mapper.Map<OutcomeViewModel>(dboutcome);
                }
                await service.UpdateRange(list);
            }
            var subjects = service.GetAll<Subject>().Where(s => s.ProfessionId == model.Profession.Id && model.Subjects.FindIndex(e => e.Id == s.Id) == -1).Select(s => s.Id).ToList();
            foreach (var s in subjects)
            {
                await service.DeleteById<Subject>(s);
            }
            return Json(new { model, redirect = Url.Action("SubjectSequences", "Subject", new { professionId = model.Profession.Id}) });
        }

        [HttpGet]
        [Authorize(Roles = "SubjectMaker")]
        public IActionResult SubjectSequences(int professionId)
        {
            if (professionId == 0) return NotFound();
            var profession = service.GetById<Profession>(professionId);
            if (profession == null) return NotFound();
            else ViewBag.Profession = mapper.Map<ProfessionViewModel>(profession);
            var model = GetSubjectSequence(professionId);
            return View("SubjectSequence", model);
        }

        public async Task<IActionResult> SaveSubjectSequences([FromBody]List<List<SubjectViewModel>> model)
        {
            if (model.Count != 8)
            {
                return NotFound();
            }
            var updatedModel = new List<Subject>();
            for (int i = 0; i < model.Count; i++)
            {
                foreach (var viewmodel in model[i])
                {
                    var subject = service.GetById<Subject>(viewmodel.Id);
                    subject.Level = i + 1;
                    updatedModel.Add(subject);
                }
            }
            await service.UpdateRange(updatedModel);
            return RedirectToAction("Index", "Subject");
        }

        private List<List<SubjectViewModel>> GetSubjectSequence(int professionId)
        {
            var finaloutcomes = service.GetAll<FinalOutCome>().Select(o => mapper.Map<FinalOutCome>(o)).ToList();
            var subjects = service.GetAll<Subject>().Where(s => s.ProfessionId == professionId).Select(s => mapper.Map<SubjectViewModel>(s)).ToList();
            var edges = service.GetAll<Edge>().Where(e => e.ProfessionId == professionId).Select(s => mapper.Map<EdgeViewModel>(s)).ToList();
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            foreach (var edge in edges)
            {
                var s1 = finaloutcomes.Find(o => o.Id == edge.FromNode).SubjectId;
                var s2 = finaloutcomes.Find(o => o.Id == edge.ToNode).SubjectId;
                if (s1.HasValue && s2.HasValue && s1 != s2)
                {
                    subjects.Find(s => s.Id == s1).DependentSubjects.Add(s2.Value);
                    left.Add(s1.Value);
                    right.Add(s2.Value);
                }
            }

            var subjectEdges = CycleRemove.RemoveCycles(left.Count, left, right);
            var graph = new Graph(subjects.Count);
            foreach (var edge in subjectEdges)
            {
                var idx1 = subjects.FindIndex(s => s.Id == edge.Item1);
                var idx2 = subjects.FindIndex(s => s.Id == edge.Item2);
                graph.AddEdge(idx1, idx2);
            }
            foreach (var edge in subjectEdges)
            {
                var idx1 = subjects.FindIndex(s => s.Id == edge.Item1);
                var idx2 = subjects.FindIndex(s => s.Id == edge.Item2);
                graph.RemoveEdge(idx1, idx2);
                if (!graph.IsReachable(idx1, idx2))
                {
                    graph.AddEdge(idx1, idx2);
                }
            }
            var edg = graph.GetEdges();
            var filteredEdges = new List<Tuple<int, int>>();
            for (int i = 0; i < edg.Count; i++)
            {
                var s1 = subjects[edg[i].Item1].Id;
                var s2 = subjects[edg[i].Item2].Id;
                filteredEdges.Add(new Tuple<int, int>(s1, s2));
            }
            var subjectLevels = new List<int>[9];
            for (int i = 0; i < 9; i++)
            {
                subjectLevels[i] = new List<int>();
            }
            var rootNodes = subjects.Select(s => s.Id).Where(i => filteredEdges.All(e => e.Item2 != i)).ToList();
            subjectLevels[0].AddRange(rootNodes);
            for (int i = 1; i < subjectLevels.Length - 1; i++)
            {
                var prevoious = subjectLevels[i - 1];
                var currunt = subjects.Select(s => s.Id).Where(s => filteredEdges.Any(e => e.Item2 == s && prevoious.Contains(e.Item1)));
                subjectLevels[i] = currunt.ToList();
            }
            subjectLevels[8] = subjects.Select(s => s.Id).Where(s => subjectLevels.All(l => l.All(v => v != s))).ToList();
            var model = new List<SubjectViewModel>[9];
            for (int i = 0; i < model.Length; ++i)
            {
                if (model[i] == null) model[i] = new List<SubjectViewModel>();
                foreach (var subject in subjectLevels[i])
                {
                    var m = mapper.Map<SubjectViewModel>(service.GetById<Subject>(subject));
                    model[i].Add(m);
                }
            }
        }

    }
}
