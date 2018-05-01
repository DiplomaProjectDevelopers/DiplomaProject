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
            var subjectlist = model.Select(sb => sb.Name).ToList();
            ViewBag.ListofSubject = subjectlist;
            return View();
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
            return Json(model);
        }

        public IActionResult GetSubjectsequence(int professionId)
        {
            var finaloutcomes = service.GetAll<FinalOutCome>().Select(o => mapper.Map<FinalOutCome>(o)).ToList();
            var subjects = service.GetAll<Subject>().Where(s => s.ProfessionId == professionId).Select(s => mapper.Map<SubjectViewModel>(s)).ToList();
            var edges = service.GetAll<Edge>().Where(e => e.ProfessionId == professionId).Select(s  => mapper.Map<EdgeViewModel>(s)).ToList();
            foreach (var edge in edges)
            {
                var s1 = finaloutcomes.Find(o => o.Id == edge.FromNode).SubjectId;
                var s2 = finaloutcomes.Find(o => o.Id == edge.ToNode).SubjectId;
                if (s1.HasValue && s2.HasValue && s1 != s2)
                {
                    subjects.Find(s => s.Id == s1).DependentSubjects.Add(s2.Value);
                }
            }
            return View();
        }
    }
}
