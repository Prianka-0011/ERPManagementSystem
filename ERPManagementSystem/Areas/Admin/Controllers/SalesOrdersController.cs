using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.serialnum = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var salesOrder = _context.SaleOrders.Where(c => c.SaleOrderStatus != "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    salesOrder = salesOrder.OrderByDescending(n => n.OrderNo);
                    break;
                default:
                    salesOrder = salesOrder.OrderBy(n => n.OrderNo);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                salesOrder = _context.SaleOrders.Where(c => c.SaleOrderStatus == "Enable" && c.OrderNo.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = salesOrder.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = salesOrder.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }
        public IActionResult DraftInvoice(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var salesOrder = _context.SaleOrders.Where(c => c.Id == id).FirstOrDefault();
            var saleOrderItems = _context.SaleOrderItems.Where(c => c.SaleOrderId == id).ToList();
            DraftInvoiceVm draft = new DraftInvoiceVm();
            draft.Id = salesOrder.Id;
            draft.DisplayName = salesOrder.DisplayName;
            draft.OrderNo = salesOrder.OrderNo;
            draft.OrderDate = salesOrder.OrderDate.ToString();
            draft.ShippingCost = salesOrder.ShippingCost;
            draft.OrderTotal = salesOrder.OrderTotal;
            draft.Address = salesOrder.Address;
            draft.Phone = salesOrder.Phone;
            draft.GrossTotal = 0;
            draft.PaymentMethod = salesOrder.PaymentMethod;
            StockProductVm prodVm;
            draft.ProductVms = new List<StockProductVm>();
            foreach (var item in saleOrderItems)
            {
                prodVm = new StockProductVm();
                prodVm.ProductName = item.ProductName;
                prodVm.SalePrice = item.Price;
                prodVm.TaxRate = item.TaxRate;
                prodVm.Discount = item.Discount;
                prodVm.Quantity = item.Quantity;
                prodVm.ProductTotal = item.ProductTotal;
                draft.GrossTotal = item.ProductTotal + draft.GrossTotal;
                draft.ProductVms.Add(prodVm);
            }

            return View(draft);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DraftInvoice(DraftInvoiceVm draftInvoiceVm)
        {
            if (ModelState.IsValid)
            {
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "INV").FirstOrDefault();
                Invoice invoice = new Invoice();
                invoice.CustomerName = draftInvoiceVm.DisplayName;
                invoice.Address = draftInvoiceVm.Address;
                invoice.Phone = draftInvoiceVm.Phone;
                invoice.InvoiceNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                serialNo.SeialNo = serialNo.SeialNo + 1;
                invoice.InvoiceDate = DateTime.Now;
                invoice.SaleOrderNo = draftInvoiceVm.OrderNo;
                invoice.DueDate = draftInvoiceVm.OrderDate;
                invoice.DueAmount = draftInvoiceVm.OrderTotal;
                invoice.PaymentMethod = draftInvoiceVm.PaymentMethod;
                invoice.TotalAmount = draftInvoiceVm.OrderTotal;
                invoice.ShippingCost = draftInvoiceVm.ShippingCost;
                invoice.PaymentStatus = "NoPaid";
                invoice.InvoiceStatus = "Pending";
                invoice.SaleOrderId = draftInvoiceVm.Id;
                _context.Invoices.Add(invoice);

                InvoiceLineItem invoiceItem;
                foreach (var item in draftInvoiceVm.ProductVms)
                {
                    invoiceItem = new InvoiceLineItem();
                    invoiceItem.InvoiceId = invoice.Id;
                    invoiceItem.ProductName = item.ProductName;
                    invoiceItem.ProductPrice = item.SalePrice;
                    invoiceItem.Quantity = item.Quantity;
                    invoiceItem.ProductTotal = item.ProductTotal;
                    _context.InvoiceLineItems.Add(invoiceItem);

                }
                var salesOrder = _context.SaleOrders.Where(c => c.Id == draftInvoiceVm.Id).FirstOrDefault();
                salesOrder.SaleOrderStatus = "Invoice Create";
                _context.Update(salesOrder);
                await _context.SaveChangesAsync();
            }

            const int pageSize = 10;
            int pg = 1;
            if (pg < 1)
            {
                pg = 1;
            }
            var invoices = _context.SaleOrders.ToList();
            var resCount = invoices.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            ViewBag.Pager = pager;
            var data = invoices.Skip(resSkip).Take(pager.PageSize);
            //return RedirectToAction("InvoiceIndex", data);
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllSalesOrders", data) });

        }
        //invoice


    }
}
