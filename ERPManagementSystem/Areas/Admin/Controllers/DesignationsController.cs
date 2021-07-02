using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DesignationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesignationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.designationnam = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var designation = _context.Designations.Where(c => c.DesignationStatus == "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    designation = designation.OrderByDescending(n => n.Name);
                    break;
                default:
                    designation = designation.OrderBy(n => n.Name);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                designation = _context.Designations.Where(c => c.DesignationStatus == "Enable" && c.Name.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = designation.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = designation.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return View(new Designation());
            }

            else
            {
                var designation = await _context.Designations.FindAsync(id);
                if (designation == null)
                {
                    return NotFound();
                }
                return View(designation);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Designation designation)
        {
            if (ModelState.IsValid)
            {
                Designation entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Designation();
                    entity.Id = Guid.NewGuid();
                    entity.Name = designation.Name;
                    entity.Salary = designation.Salary;
                    entity.DesignationStatus = designation.DesignationStatus;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Designations.FindAsync(designation.Id);
                        entity.Name = designation.Name;
                        entity.Salary = designation.Salary;
                        entity.DesignationStatus = designation.DesignationStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var designationData = _context.Designations.Where(c => c.DesignationStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = designationData.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = designationData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllDesignation", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", designation) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var designation = await _context.Designations.FindAsync(id);
            designation.DesignationStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllDesignation", _context.Designations.Where(c => c.DesignationStatus == "Enable").ToList()) });

        }
    }
}
