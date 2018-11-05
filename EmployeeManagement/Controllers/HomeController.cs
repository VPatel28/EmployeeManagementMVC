using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Controllers
{

    public class HomeController : Controller
    {
        private readonly EmployeeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(EmployeeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            if (User != null)
            {
                var user = _userManager.GetUserAsync(User);
                ViewBag.Role = user.Result.Roles;
                ViewBag.Email = user.Result.Email;
            }

            string Email = ViewBag.Email;

            var registration_Details = await _context.registration_Details
               .FirstOrDefaultAsync(m => m.Email == Email);

            //   id = Convert.ToInt32(user.Id);
            if (id == null && TempData["LoginID"] != null)
            {
                id = Convert.ToInt32(TempData["LoginID"].ToString());
                TempData.Keep("LoginID");
            }
            else
            {
                id = registration_Details.id;
                TempData["LoginID"] = id;
            }




            return View(registration_Details);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
