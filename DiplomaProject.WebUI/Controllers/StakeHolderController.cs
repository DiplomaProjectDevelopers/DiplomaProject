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

        [HttpGet]
        public IActionResult Index()
        {
            var branch = service.GetAll<Branch>().ToList();
            var branchmodel = branch.Select(b => mapper.Map<BranchViewModel>(b)).ToList();
            ViewBag.ListOfBranch = branchmodel;
            var professions = service.GetAll<Profession>().ToList();
            var proffmodel = professions.Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            ViewBag.ListofProfession = proffmodel;
            var stakeHolders = service.GetAll<StakeHolder>().ToList();
            var sthmodel = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s));
            ViewBag.ListofCompany = sthmodel;
            var stakeHoldersType = service.GetAll<StakeHolderType>().ToList();
            var sthtmodel = stakeHoldersType.Select(st => mapper.Map<StakeHolderTypeViewModel>(st)).ToList();
            ViewBag.ListOfTypeName = sthtmodel.Distinct();
            return View("Questionnaire1");
        }

        [HttpPost]
        public IActionResult Index(int branchid)
        {
            var professions = service.GetAll<Profession>().ToList();
            var proffmodel = professions.Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            var stakeHolders = service.GetAll<StakeHolder>().ToList();
            var sthmodel = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s));
            if (branchid != 0)
            {
                ViewBag.ListofProfession = proffmodel.Where(p => p.BranchId.HasValue && p.BranchId.Value == branchid);
                ViewBag.ListofCompany = sthmodel.Where(s => s.BranchId.HasValue && s.BranchId.Value == branchid);
            }
            else
            {
                var profflist = proffmodel.Select(p => p.Name).ToList();
                ViewBag.ListofProfession = profflist;
            }
            return View("Questionnaire1");
        }

       public IActionResult Outecome()
        {
            var stakeHoldersType = service.GetAll<StakeHolderType>().ToList();
            var sthtmodel = stakeHoldersType.Select(st => mapper.Map<StakeHolderTypeViewModel>(st)).ToList();
            ViewBag.ListOfTypeName = sthtmodel.Distinct();
            var subject = service.GetAll<InitialSubject>().ToList();
            var subjectmodel = subject.Select(sb => mapper.Map<InitialSubjectViewModel>(sb)).ToList();
            ViewBag.ListOfSubject = subjectmodel;
            var outcome = service.GetAll<InitialOutCome>().ToList();
            var outcomemodel = outcome.Select(o => mapper.Map<InitialOutcomeViewModel>(o)).ToList();
            ViewBag.ListOfOutcomes = outcomemodel;

            return View("Questionnaire2");
        }
    }
}