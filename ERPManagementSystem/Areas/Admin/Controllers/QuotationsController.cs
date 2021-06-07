using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuotationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public QuotationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg)
        {
            var quotation = _context.Quotations.Include(c=>c.Vendor);
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = quotation.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = quotation.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }
        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                QuotationVm quotationVm = new QuotationVm();
                quotationVm.QuotationLineItems = new List<QuotationLineItem>{ new QuotationLineItem{ Id = Guid.Parse("00000000-0000-0000-0000-000000000000") } };
                ViewData["Products"] = new SelectList(_context.Products.ToList(), "Id", "Name");
                ViewData["Tax"] = new SelectList(_context.TaxRates.ToList(), "Id", "Name");
                return View(quotationVm);
            }

            else
            {
                var quotation = await _context.Quotations.FindAsync(id);
                
                if (quotation == null)
                {
                    return NotFound();
                }
                QuotationVm quotationVm = new QuotationVm();
                quotationVm.Id = quotation.Id;
                quotationVm.QuotationNo = quotation.QuotationNo;
                quotationVm.VendorId = quotation.VendorId;
                quotationVm.Date = quotation.Date;
                quotationVm.ShippingCost = quotation.ShippingCost;
                quotationVm.QuotationLineItems = quotation.QuotationLineItems;
                return View(quotationVm);
            }

        }

        [HttpGet("/Quotations/GetProductImg")]
        public IActionResult GetProductImg(Guid id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            return Json(product);
        }
        [HttpGet("/Quotations/GetTaxRate")]
        public IActionResult GetTaxRate(Guid id)
        {
            var product = _context.TaxRates.FirstOrDefault(x => x.Id == id);
            return Json(product);
        }
    }
}
