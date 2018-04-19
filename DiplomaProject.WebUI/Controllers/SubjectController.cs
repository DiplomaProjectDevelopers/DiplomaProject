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
            return View("SubjectDistribution",fomodel);
        }
   
        //[HttpGet]
        //public IActionResult Distribution()
        //{
        //    var finaloutcome = service.GetAll<FinalOutCome>().ToList();
        //    var fomodel = finaloutcome.Select(fo => mapper.Map<FinalOutcomeViewModel>(fo)).ToList();
        //    var finaloutcomelist = fomodel.Where(o => o.SubjectId = SubjectId && o.TypeId == Wkid);
        //    var finaloutcomelist1 = fomodel.Where(o => o.SubjectId = SubjectId && o.TypeId == Wsid);
        //    var finaloutcomelist2 = fomodel.Where(o => o.SubjectId = SubjectId && o.TypeId == Waid);

        //    var sum = finaloutcomelist + finaloutcomelist1 + finaloutcomelist2;
        //    var mijin_wk = finaloutcomelist * 100 / sum;
        //    var mijin_ws = finaloutcomelist1 * 100 / sum;
        //    var mijin_wa = finaloutcomelist2 * 100 / sum;
        //    return View();
           
        //}

      

    }
}
