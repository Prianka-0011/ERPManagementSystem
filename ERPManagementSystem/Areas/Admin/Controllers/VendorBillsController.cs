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
        [NoDirectAccess]
        public async Task<IActionResult> PostBills(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.VendorBills.FindAsync(id);
            bill.VendorBillLineItems = _context.VendorBillLineItems.Where(c => c.VendorBillId == id).ToList();
            bill.GivenAmount = bill.DueAmount;
            return View(bill);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostBills(VendorBill bill)
        {

            if (ModelState.IsValid)
            {
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "CA").FirstOrDefault();

                Cash cash = new Cash();
                if (serialNo == null)
                {
                    cash.TransitionNo = "N/A";

                }

                
                cash.TransitioType = "Deduction";
                
                decimal maxValue = _context.Cashes.Max(x => x.TotalBalance);

                //if (cash.TransitioType == "Addition")
                //{
                //    cash.TotalBalance = maxValue + cash.LastTransitionAmout;
                //}
                var currentBill = _context.VendorBills.Where(c => c.Id == bill.Id).FirstOrDefault();
                if (cash.TransitioType == "Deduction")
                {
                    if (maxValue>bill.GivenAmount)
                    {
                        cash.TransitionNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                        cash.LastTransitionAmout = bill.GivenAmount;
                        cash.SourchDocNo = bill.BillNo;
                        cash.TotalBalance = maxValue - cash.LastTransitionAmout;

                        currentBill.DueAmount = currentBill.DueAmount - bill.GivenAmount;
                        if (currentBill.DueAmount > 0)
                        {
                            currentBill.BillStatus = "Posted";
                            currentBill.PaymentStatus = "Partial Paid";
                        }
                        else
                        {
                            currentBill.BillStatus = "Complete";
                            currentBill.PaymentStatus = "Paid";
                        }

                        _context.Cashes.Add(cash);
                    }
                }

               
               
                _context.VendorBills.Update(currentBill);
                await _context.SaveChangesAsync();
                var billList = _context.VendorBills.ToList();
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = billList.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = billList.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllBills", data) });

            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "PostBills", bill) });


        }

    }
}
