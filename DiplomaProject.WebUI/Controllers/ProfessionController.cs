using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    [Authorize(Roles = "DepartmentAdmin")]
    public class ProfessionController : BaseController
    {
        public ProfessionController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IEmailSender emailSender)
            : base(service, mapper, userManager, signInManager, roleManager, emailSender)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = await roleManager.FindByNameAsync("DepartmentAdmin");
            var professions = service.GetAll<UserRole>().Where(ur => ur.UserId == currentUser.Id && ur.RoleId == role.Id)
                .Select(s => mapper.Map<ProfessionViewModel>(service.GetById<Profession>(s.ProfessionId))).ToList();
            if (professions.Count == 0)
                return RedirectToAction("Index", "Home");
            for (int i = 0; i < professions.Count; i++)
            {
                if (professions[i].BranchId.HasValue && professions[i].BranchId.Value > 0)
                {
                    professions[i].Branch = service.GetById<Branch>(professions[i].BranchId.Value).Name;
                }
                if (professions[i].DepartmentId.HasValue && professions[i].DepartmentId.Value > 0)
                {
                    professions[i].Department = service.GetById<Department>(professions[i].DepartmentId.Value).Name;
                }
            }
            return View("ProfessionList", professions);
        }

        public IActionResult CreateProfession()
        {
            return View("Create");
        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment()
        {
            var role = await roleManager.FindByNameAsync("DepartmentAdmin");
            var professions = service.GetAll<UserRole>().Where(s => s.UserId == currentUser.Id && s.RoleId == role.Id).ToList();
            if (professions.Count == 0)
                return RedirectToAction("Index", "Home");
            var depid = service.GetById<Profession>(professions.First().ProfessionId).DepartmentId;
            if (!depid.HasValue || depid.Value <= 0) return RedirectToAction("Index", "Home");
            var model = mapper.Map<DepartmentViewModel>(service.GetById<Department>(depid.Value));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartment(DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var role = await roleManager.FindByNameAsync("DepartmentAdmin");
            var professions = service.GetAll<UserRole>().Where(s => s.UserId == currentUser.Id && s.RoleId == role.Id).ToList();
            if (professions.Count == 0)
                return RedirectToAction("Index", "Home");
            var depid = service.GetById<Profession>(professions.First().ProfessionId).DepartmentId.Value;
            var department = mapper.Map<Department>(model);
            department.Id = depid;
            await service.Update(department);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int professionId)
        {
            if (professionId <= 0) return RedirectToAction("Index");
            var profession = service.GetById<Profession>(professionId);
            if (profession == null) return RedirectToAction("Index");
            var model = mapper.Map<ProfessionViewModel>(profession);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var profession = mapper.Map<Profession>(model);
            profession.DepartmentId = service.GetById<Profession>(model.Id).DepartmentId;
            profession.BranchId = service.GetById<Profession>(model.Id).BranchId;
            await service.Update(profession);
            return RedirectToAction("Index");
        }
    }
}

