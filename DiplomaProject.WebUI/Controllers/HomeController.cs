﻿using System;
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Website contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
