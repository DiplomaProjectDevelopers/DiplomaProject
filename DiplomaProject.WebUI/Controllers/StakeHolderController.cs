using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class StakeHolderController : Controller
    {
        private readonly DiplomaProjectContext context;
        public StakeHolderController(DiplomaProjectContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public IActionResult Index(StakeHolder stakeHolder)
        {
            String selectValue = stakeHolder.CompanyName;
            ViewBag.SelectValue = stakeHolder.CompanyName;
            List<StakeHolder> stakeHolderList = new List<StakeHolder>();
            stakeHolderList = (from product in context.StakeHolders select product).ToList();
            stakeHolderList.Insert(0, new StakeHolder { Id = 0, CompanyName = "կազմակերպություն" });
            ViewBag.ListofStakeHolder = stakeHolderList;
            return View();
        }
    }
}
