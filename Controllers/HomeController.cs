using ESchool.AppDbContext;
using ESchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ESchoolDbContext _context;

        public HomeController(ILogger<HomeController> logger, ESchoolDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        /*custom*/

        public async Task<IActionResult> Index()
        {
            var homeCourses = await _context.Courses.Take(3).ToListAsync();
            return View(homeCourses);
        }

        public IActionResult MyCourse()
        {
            return View();
        }

        public IActionResult CourseControl()
        {
            return View();
        }


        /*custom end*/






        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
