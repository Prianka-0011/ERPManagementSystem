using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LineItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LineItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
            var brand = _context.PurchaseOrderLineItems.Include(d => d.Product);
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = brand.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = brand.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }
        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ViewData["Category"] = new SelectList(_context.Categories.ToList(), "Id", "Name");
                return View(new PurchaseOrderLineItem());
            }

            else
            {
                var brand = await _context.PurchaseOrderLineItems.FindAsync(id);
                if (brand == null)
                {
                    return NotFound();
                }
                ViewData["Category"] = new SelectList(_context.Categories.ToList(), "Id", "Name");
                return View(brand);
            }

        }

    }
}
