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
                throw new Exception();
            }
            for (int i=0; i<model.Subjects.Count;++i)
            {
                var vm = model.Subjects[i];
                var subject = mapper.Map<Subject>(vm);
                if (subject.Id > 0)
                {
                    await service.Update(subject);
                }
                else
                {
                    subject.Id = 0;
                    await service.Insert(subject);
                    model.Subjects[i] = mapper.Map<SubjectViewModel>(subject);
                }
                foreach (var outcome in vm.Outcomes)
                {
                    var fo = mapper.Map<FinalOutCome>(outcome);
                    subject.FinalOutComes.Add(fo);
                }

                for (int j = 0; j< subject.FinalOutComes.Count; ++j)
                {
                    var outcome = subject.FinalOutComes.ToList()[j];
                    outcome.SubjectId = subject.Id;
                    await service.Update(outcome);
                    model.Subjects[i].Outcomes[j] = mapper.Map<OutcomeViewModel>(outcome);
                }
            }
            return View("~/Views/Outcomes/SubjectListPreview", model);
        }
    }
}
