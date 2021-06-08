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
    public class TaxRatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaxRatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
            var taxRate = _context.TaxRates.Where(c => c.Status == "Enable");
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = taxRate.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = taxRate.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return View(new TaxRate());
            }

            else
            {
                var taxRate = await _context.TaxRates.FindAsync(id);
                if (taxRate == null)
                {
                    return NotFound();
                }
                return View(taxRate);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, TaxRate taxRate)
        {
            if (ModelState.IsValid)
            {
                TaxRate entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new TaxRate();
                    entity.Id = Guid.NewGuid();
                    entity.Name = taxRate.Name;
                    entity.Rate = taxRate.Rate;
                    entity.Status = taxRate.Status;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.TaxRates.FindAsync(taxRate.Id);
                        entity.Name = taxRate.Name;
                        entity.Rate = taxRate.Rate;
                        entity.Status = taxRate.Status;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var taxRateData = _context.TaxRates.Where(c => c.Status == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = taxRateData.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = taxRateData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllTaxRate", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", taxRate) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var taxRate = await _context.TaxRates.FindAsync(id);
            taxRate.Status = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllBrand", _context.TaxRates.Where(c => c.Status == "Enable").ToList()) });

        }
    }
}
