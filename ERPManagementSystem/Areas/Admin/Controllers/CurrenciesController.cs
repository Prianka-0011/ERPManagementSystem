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
    public class CurrenciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurrenciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.currencynam = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var currency = _context.Currencies.Where(c => c.CurrencyStatus == "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    currency = currency.OrderByDescending(n => n.CurrencyName);
                    break;
                default:
                    currency = currency.OrderBy(n => n.CurrencyName);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                currency = _context.Currencies.Where(c => c.CurrencyStatus == "Enable" && c.CurrencyName.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = currency.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = currency.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
               
                return View(new Currency());
            }

            else
            {
                var currency = await _context.Currencies.FindAsync(id);
                if (currency == null)
                {
                    return NotFound();
                }
                return View(currency);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Currency currency)
        {
            if (ModelState.IsValid)
            {
                Currency entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Currency();
                    entity.Id = Guid.NewGuid();
                    entity.CurrencyName = currency.CurrencyName;
                    entity.CurrencyStatus = currency.CurrencyStatus;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Currencies.FindAsync(currency.Id);
                        entity.CurrencyName = currency.CurrencyName;
                        entity.CurrencyStatus = currency.CurrencyStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var currencyData = _context.Currencies.Where(c => c.CurrencyStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = currencyData.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = currencyData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCurrency", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", currency) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var currency = await _context.Currencies.FindAsync(id);
            currency.CurrencyStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCurrency", _context.Currencies.Where(c => c.CurrencyStatus == "Enable").ToList()) });

        }
    }
}
