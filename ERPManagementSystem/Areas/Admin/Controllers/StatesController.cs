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
    public class StatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.statenam = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var state = _context.States.Include(c => c.Country).Where(c => c.StateStatus == "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    state = state.OrderByDescending(n => n.Name);
                    break;
                default:
                    state = state.OrderBy(n => n.Name);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                state = _context.States.Include(c => c.Country).Where(c => c.StateStatus == "Enable" && c.Name.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = state.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = state.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
                return View(new State());
            }

            else
            {
                var state = await _context.States.FindAsync(id);
                if (state == null)
                {
                    return NotFound();
                }
                ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
                return View(state);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, State state)
        {
            if (ModelState.IsValid)
            {
                State entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new State();
                    entity.Id = Guid.NewGuid();
                    entity.Name = state.Name;
                    entity.CountryId = state.CountryId;
                    entity.StateStatus = state.StateStatus;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.States.FindAsync(state.Id);
                        entity.Name = state.Name;
                        entity.CountryId = state.CountryId;
                        entity.StateStatus = state.StateStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var stateData = _context.States.Include(c => c.Country).Where(c => c.StateStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = stateData.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = stateData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllState", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", state) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var state = await _context.States.FindAsync(id);
            state.StateStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllState", _context.States.Where(c => c.StateStatus == "Enable").ToList()) });

        }
    }
}
