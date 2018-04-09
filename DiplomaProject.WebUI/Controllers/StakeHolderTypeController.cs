using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DiplomaProject.Domain.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class StakeHolderTypeController : BaseController
    {
        public StakeHolderTypeController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        public IActionResult Index()
        {
            var stakeHolderType = service.GetAll<StakeHolderType>().ToList();
            var model = stakeHolderType.Select(st => mapper.Map<StakeHolderTypeViewModel>(st));
            List<StakeHolderTypeViewModel> stakeHolderTypeList = model.Select(st => new StakeHolderTypeViewModel { Id = st.Id, TypeName = st.TypeName }).ToList();
            ViewBag.ListOfStakeHolderType = stakeHolderTypeList;
            return View("Questionnaire1", model);
        }
    }
}

