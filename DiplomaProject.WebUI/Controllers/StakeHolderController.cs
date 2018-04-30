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
using DiplomaProject.Domain.Extentions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class StakeholderController : BaseController
    {
        public StakeholderController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        //public IActionResult Index()
        //{
        //    var stakeHolders = service.GetAll<StakeHolder>().ToList();
        //    var model = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s));
        //    List<StakeHolderViewModel> stakeHoldersList = model.Select(s => new StakeHolderViewModel { CompanyName = s.CompanyName }).ToList();
        //    stakeHoldersList.Insert(0, new StakeHolderViewModel { Id = 0, CompanyName = "կազմակերպություն" });
        //    ViewBag.ListofStakeHolder = stakeHoldersList;
        //    return View("Questionnaire1", model);
        //}

        [HttpGet, ActionName("Stakeholders")]
        [Authorize]
        public async Task<IActionResult> GetStakeholders()
        {
            var user = await userManager.GetUserAsync(User);
            var role = await roleManager.FindByNameAsync(await userManager.GetRoleAsync(user));
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
                if (role.Priority <= 3) s.CanEdit = true;
            }
            var um = mapper.Map<UserViewModel>(user);
            um.Professions = service.GetAll<Profession>().Where(p => p.AdminId == user.Id).Select(p => mapper.Map<ProfessionViewModel>(p)).ToList();
            ViewBag.User = um;
            return View("StakeholderList", stakeholders);
        }

        [Authorize(Roles = "DepartmentAdmin, FacultyAdmin, BaseAdmin, ProfessionAdmin")]
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

        [Authorize(Roles = "DepartmentAdmin, FacultyAdmin, BaseAdmin, ProfessionAdmin")]
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
        [Authorize(Roles = "DepartmentAdmin, FacultyAdmin, BaseAdmin, ProfessionAdmin")]
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
            if (isDirty){
                await service.Update(stakeholder);
            }
            TempData["Message"] = Messages.STAKEHOLDER_UPDATED_SUCCESS;
            return RedirectToAction("Stakeholders", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentAdmin, FacultyAdmin, BaseAdmin, ProfessionAdmin")]
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
        [Authorize(Roles = "DepartmentAdmin, FacultyAdmin, BaseAdmin, ProfessionAdmin")]
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
                return RedirectToAction("Stakeholders", "Admin");
            }
            catch
            {
                TempData["Message"] = Messages.STAKEHOLDER_UPDATED_FAILURE;
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentAdmin, FacultyAdmin, BaseAdmin, ProfessionAdmin")]
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
        [Authorize(Roles = "DepartmentAdmin, FacultyAdmin, BaseAdmin, ProfessionAdmin")]
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
                return RedirectToAction("Stakeholders", "Admin");
            }
            catch
            {
                var model = mapper.Map<StakeHolderViewModel>(stakeholder);
                return View("Delete", model);
            }
        }



        //private readonly DiplomaProjectContext context;
        //public StakeHolderController(DiplomaProjectContext context)
        //{
        //    this.context = context;
        //}

        //[HttpPost]
        //public IActionResult Index(StakeHolder stakeHolder)
        //{
        //    String selectValue = stakeHolder.CompanyName;
        //    ViewBag.SelectValue = stakeHolder.CompanyName;
        //    List<StakeHolder> stakeHolderList = new List<StakeHolder>();
        //    stakeHolderList = (from product in context.StakeHolders select product).ToList();
        //    stakeHolderList.Insert(0, new StakeHolder { Id = 0, CompanyName = "կազմակերպություն" });
        //    ViewBag.ListofStakeHolder = stakeHolderList;
        //    return View("Questionnaire1");
        //}


        //public StakeHolderController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        //    : base(service, mapper, userManager, signInManager, roleManager)
        //{

        //}

        //public IActionResult Index()
        //{
        //    return Questionnaire();
        //}

        //[NonAction]
        //private IActionResult Questionnaire()
        //{
        //    var user = userManager.GetUserAsync(User).Result;
        //    var stakeHolders = service.GetAll<StakeHolder>().ToList();
        //    var model = stakeHolders.Where(o => user.StakeHolderTypes.Select(p => p.Id).Contains(o.TypeId.Value)).Select(o => mapper.Map<StakeHolderViewModel>(o));


        //    return View("Questionnaire1", model);
        //}
    }
}

