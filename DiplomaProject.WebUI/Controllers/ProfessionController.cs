using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class ProfessionController : Controller
    {
        private readonly DiplomaProjectContext context;
        public ProfessionController(DiplomaProjectContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult Index(ProfessionViewModel profession)
        {
            if (profession.Id == 0) { }
            String selectValue = profession.Name;
            ViewBag.SelectValue = profession.Name;
            List<ProfessionViewModel> professiօnlist = new List<ProfessionViewModel>();
            professiօnlist = (from product in context.Professions select new ProfessionViewModel() { Id = product.Id, Name = product.Name }).ToList();
            professiօnlist.Insert(0, new ProfessionViewModel { Id = 0, Name = "մասնագիտություն" });
            ViewBag.ListofProfession = professiօnlist;
            return View();
        }
    }
}
