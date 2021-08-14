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
    public class InvoiceController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> InvoiceIndex(int pg, string sortOrder, string searchString)
        {
            ViewBag.serialnum = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var invoice = _context.Invoices.Where(c => c.InvoiceStatus != "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    invoice = invoice.OrderByDescending(n => n.InvoiceNo);
                    break;
                default:
                    invoice = invoice.OrderBy(n => n.InvoiceNo);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                invoice = _context.Invoices.Where(c => c.InvoiceNo.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = invoice.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = invoice.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

    }
}
