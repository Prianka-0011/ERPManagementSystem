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
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.brandnam = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var country = _context.Countries.Where(c => c.CountryStatus == "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    country = country.OrderByDescending(n => n.Name);
                    break;
                default:
                    country = country.OrderBy(n => n.Name);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                country = _context.Countries.Where(c => c.CountryStatus == "Enable" && c.Name.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = country.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = country.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return View(new Country());
            }

            else
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound();
                }
                return View(country);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Country country)
        {
            if (ModelState.IsValid)
            {
                Country entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Country();
                    entity.Id = Guid.NewGuid();
                    entity.Name = country.Name;
                    entity.CountryStatus = country.CountryStatus;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Countries.FindAsync(country.Id);
                        entity.Name = country.Name;
                        entity.CountryStatus = country.CountryStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var countryData = _context.Countries.Where(c => c.CountryStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = countryData.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = countryData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllcountry", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", country) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            country.CountryStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllcountry", _context.Countries.Where(c => c.CountryStatus == "Enable").ToList()) });

        }
    }
}
