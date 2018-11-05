using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.DAL;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly EmployeeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeController(EmployeeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        private void PopulateDepartment(string id = "")
        {
            List<Department> departments = new List<Department>();
            departments = _context.departments.ToList();
            SelectList departmentList = new SelectList(departments, "Dept_id", "Name", true);
            if (id != "")
                departmentList.First(x => x.Text == id.ToString()).Selected = true;

            ViewBag.Department = departmentList;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            TempData.Keep("LoginID");
            int id = 0;
            if ( TempData["LoginID"] != null)
            {
                id = Convert.ToInt32(TempData["LoginID"].ToString());
                TempData.Keep("LoginID");
            }

            if (User != null)
            {
                var user = _userManager.GetUserAsync(User);
                ViewBag.Roles = user.Result.Roles;
            }
            
            return View(await _context.employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TempData.Keep("LoginID");
            var employee = await _context.employees
                .FirstOrDefaultAsync(m => m.Emp_Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            PopulateDepartment();
            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            TempData.Keep("LoginID");
            PopulateDepartment();
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Emp_Id,Name,Surname,Address,Qualification,ContactNumber,Department")] Employee employee)
        {
            TempData.Keep("LoginID");
            if (ModelState.IsValid)
            {
                Department dept = await _context.departments
                .FirstOrDefaultAsync(m => m.Dept_id == Convert.ToInt32(employee.Department));
                employee.Department = dept.Name;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                PopulateDepartment();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TempData.Keep("LoginID");
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            PopulateDepartment(employee.Department);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Emp_Id,Name,Surname,Address,Qualification,ContactNumber,Department")] Employee employee)
        {
            TempData.Keep("LoginID");
            if (id != employee.Emp_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Department dept = await _context.departments
             .FirstOrDefaultAsync(m => m.Dept_id == Convert.ToInt32(employee.Department));
                    employee.Department = dept.Name;
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Emp_Id))
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
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            TempData.Keep("LoginID");
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.employees
                .FirstOrDefaultAsync(m => m.Emp_Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TempData.Keep("LoginID");
            var employee = await _context.employees.FindAsync(id);
            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.employees.Any(e => e.Emp_Id == id);
        }
    }
}
