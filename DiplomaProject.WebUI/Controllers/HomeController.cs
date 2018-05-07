using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Համակարգի նկարագրության բաժին.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Վեբ կայքի աջակցության բաժին.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
