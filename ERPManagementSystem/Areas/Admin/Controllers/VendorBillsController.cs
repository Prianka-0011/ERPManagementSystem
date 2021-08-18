using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VendorBillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorBillsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.serialnum = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var vendorBills = _context.VendorBills.AsQueryable();
            switch (sortOrder)
            {
                case "prod_desc":
                    vendorBills = vendorBills.OrderByDescending(n => n.BillNo);
                    break;
                default:
                    vendorBills = vendorBills.OrderBy(n => n.BillNo);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                vendorBills = _context.VendorBills.Where(c => c.BillNo.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = vendorBills.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = vendorBills.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

    }
}
