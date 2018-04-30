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
            branchmodel.Insert(0, new BranchViewModel { Id = 0, Name = "ճյուղ" });
            ViewBag.ListOfBranch = branchmodel;
            var professions = service.GetAll<Profession>().ToList();
            var proffmodel = professions.Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            proffmodel.Insert(0, new ProfessionViewModel { Id = 0, Name = "մասնագիտություն" });
            ViewBag.ListofProfession = proffmodel;
            ViewBag.ListOfTypeName = service.GetAll<StakeHolderType>().Select(s => new StakeHolderTypeViewModel
            {
                Id = s.Id,
                TypeName = s.ProfessionName ?? s.TypeName
            });
            var stakeHolders = service.GetAll<StakeHolder>().ToList();
            var sthmodel = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s)).ToList();
            sthmodel.Insert(0, new StakeHolderViewModel { Id = 0, CompanyName = "կազմակերպություն" });
            ViewBag.ListofCompany = sthmodel;
            return View("Questionnaire1");
        }

        [HttpPost]
        public IActionResult IndexSel([FromForm]int branchid, string profftext, string stakeholder, int companyid)
        {
            //string[] listSTH = selectedStakeholder.Split(",");

            //foreach (var item in listSTH)
            //{

            //}
            //ViewBag.ProfessionId = data.selectedProfession;
            return View();
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
        public IActionResult Outcome([FromForm]int stakeholderid, string outcome, string weight, string id, string subject)
        {
            TempData["msg"] = "Pass";
            string[] outcomelist = outcome.Split(",");
            string[] weightstr = weight.Split(",");
            string[] idstr = id.Split(",");
            string[] subjectstr = subject.Split(",");
            int[] weightlist = new int[weightstr.Length];
            int[] idlist = new int[weightstr.Length];
            int[] subjectlist = new int[weightstr.Length];
            for (int i=0; i < weightstr.Length; i++)
            {
                weightlist[i] = int.Parse(weightstr[i]);
                idlist[i] = int.Parse(idstr[i]);
                subjectlist[i] = int.Parse(subjectstr[i]);
            }
            var stakeHolderType = service.GetAll<StakeHolderType>().ToList();
            var sthtypemodel = stakeHolderType.Select(st => mapper.Map<StakeHolderTypeViewModel>(st)).ToList();
            var coefficient = sthtypemodel.Find(st => st.Id.Equals(stakeholderid)).Coefficient.Value;
            double[] multipleofcoefficient = new double[weightlist.Length];
            for (int i = 0; i < weightlist.Length; i++)
            {
                var m = weightlist[i] * coefficient;
                multipleofcoefficient[i] = m;
            }
            
           // var outcomeinsertmodel = outcomeinsert.Select(oi => mapper.Map<OutcomeViewModel>(oi)).ToList();
            for (int i = 0; i < outcomelist.Length; i++)
            {
                var data=new OutCome
                {
                    Name = outcomelist[i],
                    OutComeTypeId = idlist[i],
                    InitialSubjectId = subjectlist[i],
                    StakeHolderId = stakeholderid,
                    Weight = weightlist[i],
                };
                var outcomeinsert = service.Insert<OutCome>(data);
            }

            var finaloutcome = service.GetAll<FinalOutCome>().ToList();
            var finaloutcomemodel = finaloutcome.Select(fo => mapper.Map<OutcomeViewModel>(fo)).ToList();
            var outcomeget = service.GetAll<OutCome>().ToList();
            var outcomemodel = outcomeget.Select(o => mapper.Map<FinalOutcomeViewModel>(o)).ToList();
            foreach (var outcomeitem in outcomemodel)
            {
                foreach (var finaloutcomeitem in finaloutcomemodel)
                {
                    if (finaloutcomeitem.Name == outcomeitem.Name)
                    {
                        if (finaloutcomeitem.TotalWeight == 0)
                        {
                            finaloutcomeitem.TotalWeight += outcomeitem.Weight;
                        }
                        else
                        {
                            finaloutcomeitem.TotalWeight = (finaloutcomeitem.TotalWeight + outcomeitem.Weight) / 2;
                        }
                        finaloutcomeitem.SubjectId = outcomeitem.InitialSubjectId;
                    }
                    else
                    {
                        var data = new FinalOutCome
                        {
                            Name = outcomeitem.Name,
                            SubjectId = outcomeitem.InitialSubjectId,
                            TotalWeight = outcomeitem.Weight,
                        };
                        var finaloutcomeinsert = service.Insert<FinalOutCome>(data);
                    }
                }
                var outcomedelate = service.DeleteById<OutCome>(outcomeitem.Id);
            }

            return View();
        }

        //[HttpPost]
        //public IActionResult FinalOutcome()
        //{
        //    var finaloutcome = service.GetAll<FinalOutCome>().ToList();
        //    var finaloutcomemodel = finaloutcome.Select(fo => mapper.Map<OutcomeViewModel>(fo)).ToList();
        //    var outcome = service.GetAll<OutCome>().ToList();
        //    var outcomemodel = outcome.Select(o => mapper.Map<FinalOutcomeViewModel>(o)).ToList();           
        //    foreach(var outcomeitem in outcomemodel)
        //    {
        //        foreach(var finaloutcomeitem in finaloutcomemodel)
        //        {
        //            if (finaloutcomeitem.Name == outcomeitem.Name)
        //            {
        //                if (finaloutcomeitem.TotalWeight == 0)
        //                {
        //                    finaloutcomeitem.TotalWeight += outcomeitem.Weight;
        //                }
        //                else
        //                {
        //                    finaloutcomeitem.TotalWeight = (finaloutcomeitem.TotalWeight + outcomeitem.Weight) / 2;
        //                }
        //                finaloutcomeitem.SubjectId = outcomeitem.InitialSubjectId;
        //            }
        //            else
        //            {
        //                var data = new FinalOutCome
        //                {
        //                    Name = outcomeitem.Name,
        //                    SubjectId = outcomeitem.InitialSubjectId,
        //                    TotalWeight = outcomeitem.Weight,
        //                };
        //                var finaloutcomeinsert = service.Insert<FinalOutCome>(data);
        //            }
        //        }
        //        var outcomedelate = service.DeleteById<OutCome>(outcomeitem.Id);
        //    }
           
        //    return View();
        //}
    }
}