using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public EmployeesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.employeenam = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var employee = _context.Employees.Include(c => c.Designation).Where(c => c.EmployeeStatus == "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    employee = employee.OrderByDescending(n => n.FirstName);
                    break;
                default:
                    employee = employee.OrderBy(n => n.FirstName);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                employee = _context.Employees.Include(c => c.Designation).Where(c => c.EmployeeStatus == "Enable" && c.FirstName.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = employee.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = employee.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //ajax call data 
        [HttpGet("/Employees/GetDesignationSalary")]
        public IActionResult GetDesignationSalary(Guid id)
        {
            var employeeSalary = _context.Designations.Where(c => c.Id == id).FirstOrDefault();
            if (employeeSalary == null)
            {
                employeeSalary = new Designation();
            }
            return Json(employeeSalary.Salary);
        }
        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles, "Id", "Name");
                return View(new EmployeeVm());
            }

            else
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                EmployeeVm employeeVm = new EmployeeVm();
                employeeVm.FirstName = employee.FirstName;
                employeeVm.LastName = employee.LastName;
                employeeVm.DesignationId = employee.DesignationId;
                employeeVm.EmployeeType = employee.EmployeeType;
                employeeVm.EmployeeStatus = employee.EmployeeStatus;
                employeeVm.Salary = employee.Salary;
                employeeVm.Phone = employee.Phone;
                employeeVm.Email = employee.Email;
                employeeVm.ReviewSalary = employee.ReviewSalary;
                employeeVm.RoleId = employee.RoleId;

                ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles, "Id", "Name");
                return View(employee);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, EmployeeVm employee)
        {
            if (ModelState.IsValid)
            {
                Employee entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Employee();
                    entity.Id = Guid.NewGuid();
                    entity.FirstName = employee.FirstName;
                    entity.LastName = employee.LastName;
                    entity.Phone = employee.Phone;
                    entity.Email = employee.Email;
                    entity.EmployeeType = employee.EmployeeType;
                    entity.Salary = employee.Salary;
                    entity.ReviewSalary = employee.ReviewSalary;
                    entity.RoleId = employee.RoleId;
                    entity.DesignationId = employee.DesignationId;
                    entity.EmployeeStatus = employee.EmployeeStatus;
                    entity.RoleId = employee.RoleId;
                    var user = new ApplicationUser { UserName = employee.UserName, Email = employee.Email };
                    var result = await _userManager.CreateAsync(user, employee.Password);
                    if (result.Succeeded)
                    {
                        _context.Add(entity);
                        await _context.SaveChangesAsync();
                        IdentityResult res = null;
                        var userRole = await _userManager.FindByIdAsync(user.Id);
                        if (!(await _userManager.IsInRoleAsync(user, employee.RoleId)))
                        {
                            var role=await _roleManager.FindByIdAsync(employee.RoleId);
                            res = await _userManager.AddToRoleAsync(user, role.Name);
                        }
                        
                        if (res.Succeeded)
                        {
                            var employeeData = _context.Employees.Include(c => c.Designation).Where(c => c.EmployeeStatus == "Enable");
                            int pg = 1;
                            const int pageSize = 10;
                            if (pg < 1)
                            {
                                pg = 1;
                            }
                            var resCount = employeeData.Count();
                            ViewBag.TotalRecord = resCount;
                            var pager = new Pager(resCount, pg, pageSize);
                            int resSkip = (pg - 1) * pageSize;
                            var data = employeeData.Skip(resSkip).Take(pager.PageSize);
                            ViewBag.Pager = pager;
                            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllEmployee", data) });

                        }
                     
                    }

                }

              
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", employee) });

        }
        [NoDirectAccess]
        public async Task<IActionResult> Edit(Guid id)
        {

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            EmployeeVm employeeVm = new EmployeeVm();
            employeeVm.FirstName = employee.FirstName;
            employeeVm.LastName = employee.LastName;
            employeeVm.DesignationId = employee.DesignationId;
            employeeVm.EmployeeType = employee.EmployeeType;
            employeeVm.EmployeeStatus = employee.EmployeeStatus;
            employeeVm.Salary = employee.Salary;
            employeeVm.Phone = employee.Phone;
            employeeVm.Email = employee.Email;
            employeeVm.ReviewSalary = employee.ReviewSalary;


            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
          
            return View(employee);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EmployeeVm employee)
        {
            if (ModelState.IsValid)
            {
                Employee entity;

                try
                {
                    entity = await _context.Employees.FindAsync(employee.Id);
                    entity.FirstName = employee.FirstName;
                    entity.LastName = employee.LastName;
                    entity.Phone = employee.Phone;
                    entity.Email = employee.Email;
                    entity.Email = employee.Email;
                    entity.EmployeeType = employee.EmployeeType;
                    entity.Salary = employee.Salary;
                    entity.ReviewSalary = employee.ReviewSalary;                   
                    entity.DesignationId = employee.DesignationId;
                    entity.EmployeeStatus = employee.EmployeeStatus;
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                  
                }
                catch (DbUpdateConcurrencyException ex)
                {

                }
                var employeeData = _context.Employees.Include(c => c.Designation).Where(c => c.EmployeeStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = employeeData.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = employeeData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllEmployee", data) });
            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", employee) });
        
        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            employee.EmployeeStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllEmployee", _context.Employees.Where(c => c.EmployeeStatus == "Enable").ToList()) });

        }
    }
}
