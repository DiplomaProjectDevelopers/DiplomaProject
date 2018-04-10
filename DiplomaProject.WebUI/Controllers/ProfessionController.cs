using AutoMapper;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomaProject.WebUI.Controllers
{
    public class ProfessionController : BaseController
    {
        public ProfessionController(IDPService service, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
            : base(service, mapper, userManager, signInManager, roleManager)
        {

        }

        [HttpPost]
        public IActionResult Index(ProfessionViewModel profession_select)
        {
            if (profession_select.Id == 0) { }
            string selectedName = profession_select.Name;
            ViewBag.SelectName = selectedName;
            var profession = service.GetAll<Profession>().ToList();
            var model = profession.Select(p => mapper.Map<ProfessionViewModel>(p));
            List<ProfessionViewModel> professionList = model.Select(p => new ProfessionViewModel { Name = p.Name }).ToList();
            professionList.Insert(0, new ProfessionViewModel { Id = 0, Name = "մասնագիտություն" });
            ViewBag.ListOfProfession = professionList;
            ProfessionViewModel selectedProfession = professionList.Find(p => p.Name == selectedName);
            int selectedId = selectedProfession.Id;
            TempData["SelectedId"] = selectedId;
            //var professionid=(from product in context.professions where product.name==selectvalue select new professionviewmodel() { id = product.id});
            //list<professionviewmodel> professiօnlist = new list<professionviewmodel>();
            //professiօnlist = (from product in context.professions select new professionviewmodel() { id = product.id, name = product.name }).tolist();
            //professiօnlist.insert(0, new professionviewmodel { id = 0, name = "մասնագիտություն" });
            //viewbag.listofprofession = professiօnlist;
            return View("Questionnaire1", model);
        }
    }
}

