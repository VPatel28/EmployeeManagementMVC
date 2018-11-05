using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.DAL;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly EmployeeContext _context;
      //  private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Registration
        public async Task<IActionResult> Index()
        {
          //  var user = await _userManager.GetUserAsync(User);
            return View(await _context.registration_Details.ToListAsync());
        }

        // GET: Registration/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration_Details = await _context.registration_Details
                .FirstOrDefaultAsync(m => m.id == id);
            if (registration_Details == null)
            {
                return NotFound();
            }

            return View(registration_Details);
        }

        // GET: Registration/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Surname,Address,Qualification,ContactNumber,Department")] Registration_Details registration_Details)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration_Details);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registration_Details);
        }

        // GET: Registration/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration_Details = await _context.registration_Details.FindAsync(id);
            if (registration_Details == null)
            {
                return NotFound();
            }
            return View(registration_Details);
        }

        // POST: Registration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Surname,Address,Qualification,ContactNumber,Department")] Registration_Details registration_Details)
        {
            if (id != registration_Details.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration_Details);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Registration_DetailsExists(registration_Details.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registration_Details);
        }

        // GET: Registration/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration_Details = await _context.registration_Details
                .FirstOrDefaultAsync(m => m.id == id);
            if (registration_Details == null)
            {
                return NotFound();
            }

            return View(registration_Details);
        }

        // POST: Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration_Details = await _context.registration_Details.FindAsync(id);
            _context.registration_Details.Remove(registration_Details);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Registration_DetailsExists(int id)
        {
            return _context.registration_Details.Any(e => e.id == id);
        }
    }
}
