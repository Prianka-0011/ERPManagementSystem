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
    public class VendorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
            var vendor = _context.Vendors.Where(c => c.VendorStatus == "Enable");
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = vendor.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = vendor.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return View(new Vendor());
            }

            else
            {
                var vendor = await _context.Vendors.FindAsync(id);
                if (vendor == null)
                {
                    return NotFound();
                }
                return View(vendor);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                Vendor entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Vendor();
                    entity.Id = Guid.NewGuid();
                    entity.FristName = vendor.FristName;
                    entity.LastName = vendor.LastName;
                    entity.Phone = vendor.Phone;
                    entity.Email = vendor.Email;
                    entity.Address = vendor.Address;
                    entity.CompanyName = vendor.CompanyName;
                    entity.DisplayName = vendor.DisplayName;
                    entity.WebSite = vendor.WebSite;
                    entity.VendorStatus = vendor.VendorStatus;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Vendors.FindAsync(vendor.Id);
                        entity.FristName = vendor.FristName;
                        entity.LastName = vendor.LastName;
                        entity.Phone = vendor.Phone;
                        entity.Email = vendor.Email;
                        entity.Address = vendor.Address;
                        entity.CompanyName = vendor.CompanyName;
                        entity.DisplayName = vendor.DisplayName;
                        entity.WebSite = vendor.WebSite;
                        entity.VendorStatus = vendor.VendorStatus;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var vendorData = _context.Vendors.Where(c => c.VendorStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = vendorData.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = vendorData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllVendor", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", vendor) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vendor = await _context.Vendors.FindAsync(id);
            vendor.VendorStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllVendor", _context.Vendors.Where(c => c.VendorStatus == "Enable").ToList()) });

        }
    }
}
