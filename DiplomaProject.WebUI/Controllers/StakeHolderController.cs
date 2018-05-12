using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using DiplomaProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DiplomaProject.Domain.Helpers;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class StakeholderController : BaseController
    {
        public StakeholderController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        [Authorize(Roles = "BaseAdmin, DepartmentAdmin, RequestSender")]
        public async Task<IActionResult> Index()
        {
            GetRoles();
            var user = await userManager.GetUserAsync(User);
            var stakeholders = service.GetAll<StakeHolder>().Select(s => mapper.Map<StakeHolderViewModel>(s)).ToList();
            foreach (var s in stakeholders)
            {
                if (s.BranchId.HasValue && s.BranchId.Value > 0)
                {
                    s.BranchName = service.GetById<Branch>(s.BranchId.Value)?.Name;
                }
                if (s.TypeId.HasValue && s.TypeId.Value > 0)
                {
                    var type = service.GetById<StakeHolderType>(s.TypeId.Value);
                    s.TypeName = type.ProfessionName ?? type.TypeName;
                }
            }

            var um = mapper.Map<UserViewModel>(user);
            um.Professions = service.GetAll<UserRole>().Where(up => up.UserId == user.Id).Select(p => p.ProfessionId).Distinct()
                .Select(s => mapper.Map<ProfessionViewModel>(service.GetById<Profession>(s))).ToList();

            var userRoles = service.GetAll<UserRole>().ToList().Where(p => p.UserId == user.Id).Select(p => mapper.Map<UserRoleViewModel>(p)).ToList();
            for (int i = 0; i < userRoles.Count; i++)
            {
                userRoles[i].ProfessionName = service.GetById<Profession>(userRoles[i].ProfessionId)?.Name;
                userRoles[i].UserName = (await userManager.FindByIdAsync(userRoles[i].UserId))?.UserName;
                userRoles[i].RoleName = (await roleManager.FindByIdAsync(userRoles[i].RoleId))?.Name;
                userRoles[i].RoleDisplayName = (await roleManager.FindByIdAsync(userRoles[i].RoleDisplayName))?.DisplayName;
            }

            um.UserRoles = userRoles;
            ViewBag.User = um;
            return View("Stakeholders", stakeholders);
        }

        public IActionResult StakeHolderList(string searchTerm = "")
        {
            GetRoles();
            var stakeholders = service.GetAll<StakeHolder>().Select(s => mapper.Map<StakeHolderViewModel>(s)).ToList();
            foreach (var s in stakeholders)
            {
                if (s.BranchId.HasValue && s.BranchId.Value > 0)
                {
                    s.BranchName = service.GetById<Branch>(s.BranchId.Value)?.Name;
                }
                if (s.TypeId.HasValue && s.TypeId.Value > 0)
                {
                    var type = service.GetById<StakeHolderType>(s.TypeId.Value);
                    s.TypeName = type.ProfessionName ?? type.TypeName;
                }
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                stakeholders = stakeholders.Where(s => s.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1 
                || s.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1
                ).ToList();
            }
            return PartialView("StakeholderList", stakeholders);
        }

        [Authorize(Roles = "DepartmentAdmin, BaseAdmin, ProfessionAdmin")]
        [HttpGet]
        public IActionResult Details(int? stakeholderId)
        {
            if (!stakeholderId.HasValue || stakeholderId.Value == 0)
                return NotFound();
            var stakeholder = service.GetById<StakeHolder>(stakeholderId.Value);
            var model = mapper.Map<StakeHolderViewModel>(stakeholder);
            model.TypeName = service.GetById<StakeHolderType>(model.TypeId.Value)?.ProfessionName ?? service.GetById<StakeHolderType>(model.TypeId.Value)?.TypeName;
            model.BranchName = service.GetById<Branch>(model.BranchId.Value)?.Name;
            return View(model);
        }

        [Authorize(Roles = "DepartmentAdmin, BaseAdmin, ProfessionAdmin")]
        [HttpGet]
        public IActionResult Edit(int? stakeholderId)
        {
            if (!stakeholderId.HasValue || stakeholderId.Value == 0)
            {
                return NotFound();
            }
            var stakeholder = service.GetById<StakeHolder>(stakeholderId.Value);
            if (stakeholder == null) return NotFound();
            var model = mapper.Map<StakeHolderViewModel>(stakeholder);
            model.TypeName = service.GetById<StakeHolderType>(model.TypeId.Value)?.ProfessionName ?? service.GetById<StakeHolderType>(model.TypeId.Value)?.TypeName;
            model.BranchName = service.GetById<Branch>(model.BranchId.Value)?.Name;
            var types = service.GetAll<StakeHolderType>().Select(s => mapper.Map<StakeHolderTypeViewModel>(s));
            foreach (var type in types)
            {
                type.TypeName = type.ProfessionName ?? type.TypeName;
            }
            ViewBag.Types = types.ToList();
            ViewBag.Branches = service.GetAll<Branch>().Select(b => mapper.Map<BranchViewModel>(b)).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DepartmentAdmin, BaseAdmin, ProfessionAdmin")]
        public async Task<IActionResult> Edit(StakeHolderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = Messages.STAKEHOLDER_UPDATED_FAILURE;
                return View(model);
            }
            bool isDirty = false;
            var stakeholder = service.GetById<StakeHolder>(model.Id);
            if (stakeholder == null)
            {
                return NotFound();
            }
            if (stakeholder.Email != model.Email)
            {
                isDirty = true;
                stakeholder.Email = model.Email;
            }
            if (stakeholder.CompanyName != model.CompanyName)
            {
                isDirty = true;
                stakeholder.CompanyName = model.CompanyName;
            }
            if (stakeholder.BranchId != model.BranchId)
            {
                isDirty = true;
                stakeholder.BranchId = model.BranchId;
            }
            if (stakeholder.Phone != model.Phone)
            {
                isDirty = true;
                stakeholder.Phone = model.Phone;
            }
            if (stakeholder.TypeId != model.TypeId)
            {
                isDirty = true;
                stakeholder.TypeId = model.TypeId;
            }
            if (isDirty)
            {
                await service.Update(stakeholder);
            }
            TempData["Message"] = Messages.STAKEHOLDER_UPDATED_SUCCESS;
            return RedirectToAction("Index", "Stakeholder");
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentAdmin, BaseAdmin, ProfessionAdmin")]
        public IActionResult Create()
        {
            var types = service.GetAll<StakeHolderType>().Select(s => mapper.Map<StakeHolderTypeViewModel>(s)).ToList();
            for (int i = 0; i < types.Count; i++)
            {
                types[i].TypeName = types[i].ProfessionName ?? types[i].TypeName;

            }
            ViewBag.Types = types.ToList();
            ViewBag.Branches = service.GetAll<Branch>().Select(b => mapper.Map<BranchViewModel>(b)).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "DepartmentAdmin, BaseAdmin, ProfessionAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StakeHolderViewModel model)
        {
            if (model == null || !ModelState.IsValid)
                return View(model);
            var data = mapper.Map<StakeHolder>(model);
            data.Id = 0;
            try
            {
                await service.Insert(data);
                TempData["Message"] = Messages.STAKEHOLDER_UPDATED_SUCCESS;
                return RedirectToAction("Index", "Stakeholder");
            }
            catch
            {
                TempData["Message"] = Messages.STAKEHOLDER_UPDATED_FAILURE;
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentAdmin, BaseAdmin, ProfessionAdmin")]
        public IActionResult Delete(int? stakeholderId)
        {
            if (!stakeholderId.HasValue || stakeholderId.Value == 0)
            {
                return NotFound();
            }
            var data = service.GetById<StakeHolder>(stakeholderId.Value);
            if (data == null)
            {
                return NotFound();
            }
            var model = mapper.Map<StakeHolderViewModel>(data);
            model.TypeName = service.GetById<StakeHolderType>(model.TypeId.Value)?.ProfessionName ?? service.GetById<StakeHolderType>(model.TypeId.Value)?.TypeName;
            model.BranchName = service.GetById<Branch>(model.BranchId.Value)?.Name;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DepartmentAdmin, BaseAdmin, ProfessionAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int? stakeholderId)
        {
            if (!stakeholderId.HasValue || stakeholderId.Value == 0)
                return NotFound();
            var stakeholder = service.GetById<StakeHolder>(stakeholderId.Value);
            if (stakeholder == null) return NotFound();
            try
            {
                TempData["Message"] = Messages.STAKEHOLDER_DELETED_SUCCESS;
                await service.DeleteById<StakeHolder>(stakeholderId.Value);
                return RedirectToAction("Index", "Stakeholder");
            }
            catch
            {
                var model = mapper.Map<StakeHolderViewModel>(stakeholder);
                return View("Delete", model);
            }
        }

        [HttpGet]
        public IActionResult Question()
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
            var sthmodel = stakeHolders.Where(s=>s.CompanyName!=null).Select(s => mapper.Map<StakeHolderViewModel>(s)).ToList();
            ViewBag.ListofCompany = sthmodel;
            return View("Questionnaire1");
        }

        [HttpPost]
        public IActionResult IndexSel([FromForm]int branchid, string profftext, string stakeholderstr, string companyname, string gmail, string pass)
        {
            //convert string to string array
            string[] listSTH = stakeholderstr.Split(",");
            int[] stakeholderid = new int[listSTH.Length];
            List<string> email = new List<string>();
            List<string> name = new List<string>();
            var i = 0;
            foreach (var item in listSTH)
            {
                stakeholderid[i] = int.Parse(item);
                i++;
            }
            var stakeholder = service.GetAll<StakeHolder>().ToList();
            var stakeholdermodel = stakeholder.Select(sh => mapper.Map<StakeHolderViewModel>(sh)).ToList();

            for (var j = 0; j < stakeholderid.Length; j++)
            {
                var emailAddress = stakeholdermodel.Where(sh => sh.BranchId == branchid && sh.CompanyName == companyname && sh.TypeId == stakeholderid[j]).FirstOrDefault();
                if (emailAddress != null)
                {
                    email.Add(emailAddress.Email);
                    name.Add(emailAddress.FirstName + " " + emailAddress.LastName);
                }
            }

            if (email.Count != 0)
            {
                SendEmail(gmail, pass, email, name);
            }
            return RedirectToAction("Question");
        }


        protected void SendEmail(string gmail, string pass, List<string> email, List<string> name)
        {
            for (var j = 0; j < email.Count; j++)
            {
                using (MailMessage mm = new MailMessage(gmail, email[j]))
                {
                    mm.Subject = "Հարցաթերթիկ";
                    mm.Body = "Հարգելի " + name[j] + " խնդրում ենք տրամադրել մի փոքր ժամանակ և լրացնել հետևյալ հղումով տրված հարցաթերթիկը։" +
                        " Նախապես շնորհակալություն\n" + "http://localhost:59468/StakeHolder/Outcome." + "\n\n\nՀարգանքներով` "
                        + currentUser.FirstName + " " + currentUser.LastName;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(gmail, pass);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    try
                    {
                        smtp.Send(mm);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return;
                    }
                    Console.WriteLine("Send!!");
                }
            }
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
        public IActionResult Outcome([FromForm]int stakeholderid, string outcomeName, string weight, string outcomeid, string subject)
        {
            string[] outcomelist = outcomeName.Split(",");
            string[] weightstr = weight.Split(",");
            string[] idstr = outcomeid.Split(",");
            string[] subjectstr = subject.Split(",");
            int[] weightlist = new int[weightstr.Length];
            int[] idlist = new int[weightstr.Length];
            int[] subjectlist = new int[weightstr.Length];
            for (int i = 0; i < weightstr.Length; i++)
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
            for (int i = 0; i < outcomelist.Length; i++)
            {
                var data = new OutCome
                {
                    Name = outcomelist[i],
                    OutComeTypeId = idlist[i],
                    InitialSubjectId = subjectlist[i],
                    StakeHolderId = stakeholderid,
                    Weight = multipleofcoefficient[i],
                };
                var outcomeinsert = service.Insert<OutCome>(data);
            }
            return RedirectToAction("FinalOutcomeFunc");
        }

        public ActionResult FinalOutcomeFunc()
        {
            var finaloutcome = service.GetAll<FinalOutCome>().ToList();
            var finaloutcomemodel = finaloutcome.Select(f => mapper.Map<OutcomeViewModel>(f)).ToList();
            var outcome = service.GetAll<OutCome>().ToList();
            var outcomemodel = outcome.Select(o => mapper.Map<FinalOutcomeViewModel>(o)).ToList();
            string[] arrname = new string[outcomemodel.Count];
            double[] arrweight = new double[outcomemodel.Count];
            int[] arrsubject = new int[outcomemodel.Count];
            int i = 0;
            foreach (var outcomeitem in outcomemodel)
            {
                arrsubject[i] =(int) outcomeitem.InitialSubjectId;
                arrname[i] = outcomeitem.Name;
                arrweight[i] = 0;
                foreach(var outcomeitem1 in outcomemodel)
                {
                    if (outcomeitem1.Name == outcomeitem.Name && outcomeitem1.InitialSubjectId==outcomeitem.InitialSubjectId)
                    {
                        arrweight[i] += outcomeitem1.Weight;
                        service.DeleteById<OutCome>(outcomeitem1.Id);
                    }
                }
                i++;
            }
            int j = 0;
            while (j < arrsubject.Length)
            {
                int count = 0;
                foreach (var finaloutcomeitem in finaloutcomemodel)
                {
                    if (finaloutcomeitem.Name == arrname[j] && finaloutcomeitem.InitialSubjectId == arrsubject[j])
                    {
                        var update = finaloutcome.Where(fo => fo.Name == arrname[j] && fo.InitialSubjectId == arrsubject[j]).FirstOrDefault();
                        if (update != null)
                        {
                            double sum = 0;
                            if (update.TotalWeight == null)
                            {
                                sum = Math.Round(arrweight[j], 2);
                            }
                            else
                            {
                                sum =(float) (update.TotalWeight + arrweight[j]) / 2;
                            }
                            update.TotalWeight = sum;
                            var itemupdate = service.Update<FinalOutCome>(update);
                            j++;
                            break;
                        }
                    }
                    else
                    {
                        count++;
                        if (count == finaloutcomemodel.Count)
                        {
                            var data = new FinalOutCome
                            {
                                Name = arrname[j],
                                SubjectId = arrsubject[j],
                                TotalWeight = arrweight[j],
                            };
                            var iteminsert = service.Insert<FinalOutCome>(data);
                            j++;
                        }
                    }
                }
            }
            return View("Questionnaire2");
        }
    }
}