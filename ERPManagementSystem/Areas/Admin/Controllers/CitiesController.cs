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
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.citynam = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var city = _context.Cities.Include(c => c.State).Where(c => c.CityStatus == "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    city = city.OrderByDescending(n => n.Name);
                    break;
                default:
                    city = city.OrderBy(n => n.Name);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                city = _context.Cities.Include(c => c.State).Where(c => c.CityStatus == "Enable" && c.Name.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = city.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = city.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");
                return View(new City());
            }

            else
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound();
                }
                ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");
                return View(city);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, City city)
        {
            if (ModelState.IsValid)
            {
                City entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new City();
                    entity.Id = Guid.NewGuid();
                    entity.Name = city.Name;
                    entity.StateId = city.StateId;
                    entity.CityStatus = city.CityStatus;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Cities.FindAsync(city.Id);
                        entity.Name = city.Name;
                        entity.StateId = city.StateId;
                        entity.CityStatus = city.CityStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var cityData = _context.Cities.Include(c => c.State).Where(c => c.CityStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = cityData.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = cityData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCity", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", city) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var city = await _context.Cities.FindAsync(id);
            city.CityStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCity", _context.Cities.Where(c => c.CityStatus == "Enable").ToList()) });

        }
    }
}
