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
            var sbmodel = subject.Select(sb => mapper.Map<SubjectViewModel>(sb)).ToList();
            var subjectlist = sbmodel.Select(sb => sb.Name).ToList();
            ViewBag.ListofSubject = subjectlist;
            var finaloutcome = service.GetAll<FinalOutCome>().ToList();
            var fomodel = finaloutcome.Select(fo => mapper.Map<FinalOutcomeViewModel>(fo));
            ViewBag.ListOfFinalOutcome = finaloutcome;
            return View("SubjectDistribution", fomodel);
        }

        [HttpGet]
        public IActionResult Distribution(int professionId)
       
        {
            var finalSubjects = service.GetAll<Subject>().Where(s => s.ProfessionId == professionId).ToList();
            for (int i = 0; i < finalSubjects.Count; i++)
            {

                var outcomes = service.GetAll<FinalOutCome>().Where(o => o.SubjectId == finalSubjects[i].Id);
                var giteliq = outcomes.Where(o => o.TypeId == 1);
                var karoxutyun = outcomes.Where(o => o.TypeId == 2);
                var hmtutyun = outcomes.Where(o => o.TypeId == 3);
                var credit = 0;
                var totalHours = 0;
                var gWeight = giteliq.Sum(g => g.TotalWeight);
                var kWeight = karoxutyun.Sum(k => k.TotalWeight);
                var hWeight = hmtutyun.Sum(h => h.TotalWeight);
                var sum = gWeight + kWeight + hWeight;
                finalSubjects[i].Credit = credit;
                finalSubjects[i].TotalHours = totalHours;
                service.Update(finalSubjects[i]);

                var mG = gWeight * 100 / sum;  //popoxakan 
                var mK = kWeight * 100 / sum;
                var mH = hWeight * 100 / sum;
                credit = 30 * sum / CourseHourse; // grel sa hashvi arac praktikan ev lekciayi u mnacaci jamery amen ararkayi hamar
                totalHours = credit / gWeight + credit / kWeight + credit / hWeight;
                return View();

            }



        }
    }
}
