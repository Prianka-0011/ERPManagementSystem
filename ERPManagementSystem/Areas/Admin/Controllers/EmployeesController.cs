using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
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

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
           
            var employee = _context.Employees.Include(c=>c.Designation).Where(c => c.EmployeeStatus == "Enable");
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = employee.Count();
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
            return Json(employeeSalary.Salary);
        }
        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
                return View(new Employee());
            }

            else
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
                return View(employee);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Employee employee)
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
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
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
                        entity.RoleId = employee.RoleId;
                        entity.DesignationId = employee.DesignationId;
                        entity.EmployeeStatus = employee.EmployeeStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var employeeData = _context.Employees.Include(c=>c.Designation).Where(c => c.EmployeeStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = employeeData.Count();
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
