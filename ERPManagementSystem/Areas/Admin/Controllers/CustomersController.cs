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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
            var customer = _context.Customers.Where(c => c.Status == "Enable");
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = customer.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = customer.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return View(new Customer());
            }

            else
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Customer customer)
        {
            if (ModelState.IsValid)
            {
                Customer entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Customer();
                    entity.Id = Guid.NewGuid();
                    entity.FirstName = customer.FirstName;
                    entity.LastName = customer.LastName;
                    entity.Phone = customer.Phone;
                    entity.Email = customer.Email;
                    entity.Address = customer.Address;
                    entity.Status = customer.Status;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Customers.FindAsync(customer.Id);
                        entity.FirstName = customer.FirstName;
                        entity.LastName = customer.LastName;
                        entity.Phone = customer.Phone;
                        entity.Email = customer.Email;
                        entity.Address = customer.Address;
                        entity.Status = customer.Status;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var customerData = _context.Customers.Where(c => c.Status == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = customerData.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = customerData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCustomer", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", customer) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            customer.Status = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCustomer", _context.Customers.Where(c => c.Status == "Enable").ToList()) });

        }
    }
}
