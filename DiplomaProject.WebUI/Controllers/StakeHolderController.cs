using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using DiplomaProject.Domain.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class StakeHolderController : BaseController
    {
        public StakeHolderController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        public class Data
        {
            public string selectedBranch { get; set; }
            public string selectedProfession { get; set; }
            public string selectedStakeholder { get; set; }
            public string selectedCompany { get; set; }
        }

        public class DataSecond
        {
            public string name { get; set; }
            public double value { get; set; }
            public int branch { get; set; }
        }

        [HttpGet]
        public IActionResult Index()
        {
            var branch = service.GetAll<Branch>().ToList();
            var branchmodel = branch.Select(b => mapper.Map<BranchViewModel>(b)).ToList();
            ViewBag.ListOfBranch = branchmodel;
            var professions = service.GetAll<Profession>().ToList();
            var proffmodel = professions.Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            ViewBag.ListofProfession = proffmodel;
            ViewBag.ListOfTypeName = service.GetAll<StakeHolderType>().Select(s => new StakeHolderTypeViewModel
            {
                Id = s.Id,
                TypeName = s.ProfessionName ?? s.TypeName
            });
            var stakeHolders = service.GetAll<StakeHolder>().ToList();
            var sthmodel = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s));
            ViewBag.ListofCompany = sthmodel;
            return View("Questionnaire1");
        }

        [HttpPost]
        public JsonResult IndexSel(Data data)
        {
            string[] listSTH = data.selectedStakeholder.Split(",");

            foreach (var item in listSTH)
            {

            }
            ViewBag.ProfessionId = data.selectedProfession;
            return Json("");
        }

        [HttpGet]
       public IActionResult Outcome()
        {
            ViewBag.ListOfTypeName = service.GetAll<StakeHolderType>().Select(s => new StakeHolderTypeViewModel
            {
                Id = s.Id,
                TypeName = s.ProfessionName ?? s.TypeName
            });
            var subject = service.GetAll<InitialSubject>().ToList();
            var subjectmodel = subject.Select(sb => mapper.Map<InitialSubjectViewModel>(sb)).ToList();
            ViewBag.ListOfSubject = subjectmodel;
            var outcome = service.GetAll<InitialOutCome>().ToList();
            var outcomemodel = outcome.Select(o => mapper.Map<InitialOutcomeViewModel>(o)).ToList();
            ViewBag.ListOfOutcomes = outcomemodel;

            return View("Questionnaire2");
        }

        [HttpPost]
        public IActionResult Outcome(DataSecond dataSecond)
        {
            var stakeHolders = service.GetAll<StakeHolder>().ToList();
            var sthmodel = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s)).ToList();
            var typeid = sthmodel.Find(s => s.BranchId.Equals(dataSecond.branch)).S
            var stakeHolderType = service.GetAll<StakeHolderType>().ToList();
            var sthtypemodel = stakeHolderType.Select(st => mapper.Map<StakeHolderTypeViewModel>(st)).ToList();
            var coefficient = sthtypemodel.Find(st => st.Id.Equals(typeid)).Select(st => st.Coefficient.Value);
            int outcomeWeight = dataSecond.value * coefficient;
            return View();
        }
    }
}