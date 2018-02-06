using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject.WebUI.Controllers
{
    public class OutcomesController : Controller
    {
        private IDPRepository repository;
        public OutcomesController(IDPRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Index(int professionId)
        {
            return View();
        }
    }
}