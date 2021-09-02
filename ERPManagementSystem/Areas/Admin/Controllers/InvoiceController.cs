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
            var invoice = _context.Invoices.AsQueryable();
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
        [NoDirectAccess]
        public async Task<IActionResult> PostInvoice(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            invoice.InvoiceLineItems = _context.InvoiceLineItems.Where(c => c.InvoiceId == id).ToList();
            invoice.ReceiveAmount = invoice.DueAmount;
            return View(invoice);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostInvoice(Invoice invoice)
        {
          
            if (ModelState.IsValid)
            {
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "CA").FirstOrDefault();

                Cash cash = new Cash();
                if (serialNo == null)
                {
                    cash.TransitionNo = "N/A";

                }

                cash.TransitionNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                cash.TransitioType = "Addition";
                cash.LastTransitionAmout = invoice.ReceiveAmount;
                var check = _context.Cashes.ToList();
                if (check.Count == 0)
                {
                    cash.TotalBalance = cash.LastTransitionAmout;
                    _context.Cashes.Add(cash);
                }
                else
                {
                    decimal maxValue = _context.Cashes.Max(x => x.TotalBalance);

                    if (cash.TransitioType == "Addition")
                    {
                        cash.TotalBalance = maxValue + cash.LastTransitionAmout;
                    }
                    //if (cash.TransitioType == "Deduction")
                    //{
                    //    cash.TotalBalance = maxValue - cash.LastTransitionAmout;
                    //}
                }
                
                cash.SourchDocNo = invoice.InvoiceNo;

                var currentInvoice = _context.Invoices.Where(c => c.Id == invoice.Id).FirstOrDefault();
                currentInvoice.DueAmount = currentInvoice.DueAmount - invoice.ReceiveAmount;
                if (currentInvoice.DueAmount>0)
                {
                    currentInvoice.InvoiceStatus = "Posted";
                    currentInvoice.PaymentStatus = "Partial Paid";
                }
                else
                {
                    currentInvoice.InvoiceStatus = "Complete";
                    currentInvoice.PaymentStatus = "Paid";
                }
                _context.Invoices.Update(currentInvoice);
                _context.Cashes.Add(cash);
              await  _context.SaveChangesAsync();
                var invoicesList = _context.Invoices.ToList();
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = invoicesList.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = invoicesList.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllInvoice", data) });

            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "PostInvoice", invoice) });


        }

    }
}
