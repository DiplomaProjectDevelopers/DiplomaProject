using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class StakeHolderTypeController : Controller
    {
        private readonly DiplomaProjectContext context;
        public StakeHolderTypeController(DiplomaProjectContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<StakeHolderType> stakeHolderTypeList = new List<StakeHolderType>();
            stakeHolderTypeList = (from product in context.StakeHolderTypes select product).ToList();
            return View(stakeHolderTypeList);
        }
    }
}
