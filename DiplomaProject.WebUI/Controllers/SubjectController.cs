using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.ViewModels;

namespace DiplomaProject.WebUI.Controllers
{
    public class SubjectController : Controller
    {
        private readonly DiplomaProjectContext context;
        public SubjectController(DiplomaProjectContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult Index(SubjectViewModel profession)
        {
            List<SubjectViewModel> subjectlist = new List<SubjectViewModel>();
            subjectlist = (from product in context.Professions select new SubjectViewModel() { Id = product.Id, Name = product.Name }).ToList();
            ViewBag.ListofSubject = subjectlist;
            return View();
        }
    }
}