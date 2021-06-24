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
    public class ShippingChargesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingChargesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
            var shippingCharge = _context.ShippingCharges.Where(c => c.ShippingChargeStatus == "Enable");
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = shippingCharge.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = shippingCharge.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return View(new ShippingCharge());
            }

            else
            {
                var shippingCharge = await _context.ShippingCharges.FindAsync(id);
                if (shippingCharge == null)
                {
                    return NotFound();
                }
                return View(shippingCharge);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, ShippingCharge shippingCharge)
        {
            if (ModelState.IsValid)
            {
                ShippingCharge entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new ShippingCharge();
                    entity.Id = Guid.NewGuid();
                    entity.BaseCharge = shippingCharge.BaseCharge;
                    entity.IncreaeChargePerProduct = shippingCharge.IncreaeChargePerProduct;
                    entity.ShippingChargeStatus = shippingCharge.ShippingChargeStatus;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.ShippingCharges.FindAsync(shippingCharge.Id);
                        entity.BaseCharge = shippingCharge.BaseCharge;
                        entity.IncreaeChargePerProduct = shippingCharge.IncreaeChargePerProduct;
                        entity.ShippingChargeStatus = shippingCharge.ShippingChargeStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var shippingChargeData = _context.ShippingCharges.Where(c => c.ShippingChargeStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = shippingChargeData.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = shippingChargeData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllShippingCharge", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", shippingCharge) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var shippingCharge = await _context.ShippingCharges.FindAsync(id);
            shippingCharge.ShippingChargeStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllShippingCharge", _context.ShippingCharges.Where(c => c.ShippingChargeStatus == "Enable").ToList()) });

        }
    }
}
