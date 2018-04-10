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

        public IActionResult Index()
        {
            var stakeHolders = service.GetAll<StakeHolder>().ToList();
            var model = stakeHolders.Select(s => mapper.Map<StakeHolderViewModel>(s));
            List<StakeHolderViewModel> stakeHoldersList = model.Select(s => new StakeHolderViewModel { CompanyName = s.CompanyName }).ToList();
            stakeHoldersList.Insert(0, new StakeHolderViewModel { Id = 0, CompanyName = "կազմակերպություն" });
            ViewBag.ListofStakeHolder = stakeHoldersList;
            return View("Questionnaire1", model);
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

