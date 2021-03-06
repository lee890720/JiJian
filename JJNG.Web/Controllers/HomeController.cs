﻿using JJNG.Data;
using JJNG.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JJNG.Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (User.IsInRole("Admins"))
                return Redirect("/Finance/FncFrontCalendar");
            if (User.IsInRole("人事"))
                return Redirect("/Finance/FncFrontCalendar");
            if (User.IsInRole("财务"))
                return Redirect("/Finance/FncFrontCalendar");
            if (User.IsInRole("管家"))
                return Redirect("/Branch/FrontCalendar");
            if (User.IsInRole("前台")|| User.IsInRole("前台审核"))
                return Redirect("/Branch/FrontCalendar");
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
