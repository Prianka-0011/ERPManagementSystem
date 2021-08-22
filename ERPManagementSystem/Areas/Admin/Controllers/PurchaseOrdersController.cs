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
    public class PurchaseOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.serialnum = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var purchaseOrder = _context.PurchaseOrders.Include(c => c.Vendor).AsQueryable();
            switch (sortOrder)
            {
                case "prod_desc":
                    purchaseOrder = purchaseOrder.OrderByDescending(n => n.PurchaseNo);
                    break;
                default:
                    purchaseOrder = purchaseOrder.OrderBy(n => n.PurchaseNo);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                purchaseOrder = _context.PurchaseOrders.Include(c => c.Vendor).Where(c => c.PurchaseOrderStatus == "Enable" && c.PurchaseNo.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = purchaseOrder.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = purchaseOrder.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }
        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                PurchaseOrder purchaseOrderVm = new PurchaseOrder();
                purchaseOrderVm.PurchaseOrderLineItems = new List<PurchaseOrderLineItem> { new PurchaseOrderLineItem { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), ImgPath = "images/noimg.png" } };
                ViewData["Products"] = new SelectList(_context.Products.ToList(), "Id", "Name");
                ViewData["Currency"] = new SelectList(_context.Currencies.ToList(), "Id", "CurrencyName");
                ViewData["Tax"] = new SelectList(_context.TaxRates.ToList(), "Id", "Name");
                ViewData["Vendor"] = new SelectList(_context.Vendors.ToList(), "Id", "DisplayName");
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "PO").FirstOrDefault();               
                purchaseOrderVm.PurchaseNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                serialNo.SeialNo = serialNo.SeialNo + 1;
                _context.SaveChanges();
                return View(purchaseOrderVm);
            }

            else
            {
                var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

                if (purchaseOrder == null)
                {
                    return NotFound();
                }
                PurchaseOrder purchaseOrderVm = new PurchaseOrder();
                //var PurchaseOrderLineItems = _context.PurchaseOrderLineItems.Where(c => c.PurchaseOrderId == purchaseOrder.Id).ToList();
                purchaseOrderVm.Id = purchaseOrder.Id;
                purchaseOrderVm.PurchaseNo = purchaseOrder.PurchaseNo;
                purchaseOrderVm.VendorId = purchaseOrder.VendorId;
                purchaseOrderVm.OrderDate = purchaseOrder.OrderDate;
                purchaseOrderVm.DeliveryDate = purchaseOrder.DeliveryDate;
                purchaseOrderVm.ShippingCost = purchaseOrder.ShippingCost;
                purchaseOrderVm.Discont = purchaseOrder.Discont;
                purchaseOrderVm.TotalAmount = purchaseOrder.TotalAmount;
                purchaseOrderVm.PurchaseOrderLineItems =_context.PurchaseOrderLineItems.Include(c=>c.Product).Where(c=>c.PurchaseOrderId==purchaseOrder.Id).ToList();
                purchaseOrderVm.CurrencyId = purchaseOrder.CurrencyId;
                ViewData["Products"] = new SelectList(_context.Products.ToList(), "Id", "Name");
                ViewData["Tax"] = new SelectList(_context.TaxRates.ToList(), "Id", "Name");
                ViewData["Vendor"] = new SelectList(_context.Vendors.ToList(), "Id", "DisplayName");
                ViewData["Currency"] = new SelectList(_context.Currencies.ToList(), "Id", "CurrencyName");
                return View(purchaseOrderVm);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, PurchaseOrder purchaseOrderVm)
        {

            PurchaseOrder entity;

            PurchaseOrderLineItem lineItem;
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {

                entity = new PurchaseOrder();
                entity.PurchaseNo = purchaseOrderVm.PurchaseNo;
                entity.VendorId = purchaseOrderVm.VendorId;
                entity.ShippingCost = purchaseOrderVm.ShippingCost;
                entity.OrderDate = DateTime.Now;
                entity.DeliveryDate = purchaseOrderVm.DeliveryDate;
                entity.PurchaseOrderStatus = purchaseOrderVm.PurchaseOrderStatus;
                entity.Discont = purchaseOrderVm.Discont;
                entity.TotalAmount = purchaseOrderVm.TotalAmount;
                entity.CurrencyId = purchaseOrderVm.CurrencyId;
                entity.PurchaseOrderStatus = "Enable";
                _context.PurchaseOrders.Add(entity);

                //await _context.SaveChangesAsync();
                foreach (var item in purchaseOrderVm.PurchaseOrderLineItems)
                {
                    lineItem = new PurchaseOrderLineItem();
                    lineItem.ProductId = item.ProductId;
                    lineItem.Color = item.Color;
                    lineItem.Size = item.Size;
                    lineItem.Price = item.Price;
                    lineItem.TaxRateId = item.TaxRateId;
                    lineItem.Rate = item.Rate;
                    lineItem.PurchaseOrderId = entity.Id;
                    lineItem.PerProductCost = item.PerProductCost;
                    lineItem.Discount = item.Discount;
                    lineItem.OrderQuantity = item.OrderQuantity;
                    lineItem.DueQuantity = item.OrderQuantity;
                    lineItem.ReceiveQuantity = 0;
                    lineItem.Description = item.Description;
                    lineItem.ImgPath = item.ImgPath;
                    lineItem.TotalCost = item.TotalCost;

                    lineItem.ItemStatus ="Enable";
                    _context.PurchaseOrderLineItems.Add(lineItem);
                }
             
            }

            else
            {
                try
                {
                    entity = await _context.PurchaseOrders.FindAsync(id);
                    entity.PurchaseNo = purchaseOrderVm.PurchaseNo;
                    entity.VendorId = purchaseOrderVm.VendorId;
                    entity.ShippingCost = purchaseOrderVm.ShippingCost;
                    entity.PurchaseOrderStatus = purchaseOrderVm.PurchaseOrderStatus;
                    entity.Discont = purchaseOrderVm.Discont;
                    entity.DeliveryDate = purchaseOrderVm.DeliveryDate;
                    entity.PurchaseOrderStatus = "Enable";
                    entity.TotalAmount = purchaseOrderVm.TotalAmount;
                    entity.CurrencyId = purchaseOrderVm.CurrencyId;
                   
                    foreach (var item in purchaseOrderVm.PurchaseOrderLineItems)
                    {
                        var oldLineIetm =  _context.PurchaseOrderLineItems.Where(c => c.Id == item.Id).FirstOrDefault();
                        if (oldLineIetm != null)
                        {
                            oldLineIetm.ProductId = item.ProductId;
                            oldLineIetm.Color = item.Color;
                            oldLineIetm.Size = item.Size;
                            oldLineIetm.Price = item.Price;
                            oldLineIetm.TaxRateId = item.TaxRateId;
                            oldLineIetm.Rate = item.Rate;
                            oldLineIetm.PurchaseOrderId = entity.Id;
                            oldLineIetm.PerProductCost = item.PerProductCost;
                            oldLineIetm.Discount = item.Discount;
                            oldLineIetm.OrderQuantity = item.OrderQuantity;
                            //oldLineIetm.DueQuantity = item.OrderQuantity;
                            oldLineIetm.Description = item.Description;
                            oldLineIetm.ImgPath = item.ImgPath;
                            oldLineIetm.TotalCost = item.TotalCost;
                            _context.PurchaseOrderLineItems.Update(oldLineIetm);
                        }
                        else
                        {


                            lineItem = new PurchaseOrderLineItem();
                            lineItem.ProductId = item.ProductId;
                            lineItem.Color = item.Color;
                            lineItem.Size = item.Size;
                            lineItem.Price = item.Price;
                            lineItem.TaxRateId = item.TaxRateId;
                            lineItem.Rate = item.Rate;
                            lineItem.PurchaseOrderId = entity.Id;
                            lineItem.PerProductCost = item.PerProductCost;
                            lineItem.Discount = item.Discount;
                            lineItem.OrderQuantity = item.OrderQuantity;
                            lineItem.DueQuantity = item.OrderQuantity;
                            lineItem.Description = item.Description;
                            lineItem.ImgPath = item.ImgPath;
                            lineItem.TotalCost = item.TotalCost;
                            lineItem.ItemStatus = "Enable";
                            _context.PurchaseOrderLineItems.Add(lineItem);
                        }
                    }
                    _context.Update(entity);

                }
                catch (DbUpdateConcurrencyException ex)
                {

                }
            }
            await _context.SaveChangesAsync();
            const int pageSize = 10;
            int pg = 1;
            if (pg < 1)
            {
                pg = 1;
            }
            var resultPurchaseOrder = _context.PurchaseOrders.Include(d=>d.Vendor).ToList();
            var resCount = resultPurchaseOrder.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            ViewBag.Pager = pager;
            var data = resultPurchaseOrder.Skip(resSkip).Take(pager.PageSize);

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllPurchaseOrders", data) });

            // return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit",quotationVm) });

        }
        [HttpGet("/PurchaseOrders/GetProductImg")]
        public IActionResult GetProductImg(Guid id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            return Json(product);
        }
        [HttpGet("/PurchaseOrders/GetTaxRate")]
        public IActionResult GetTaxRate(Guid id)
        {
            var product = _context.TaxRates.FirstOrDefault(x => x.Id == id);
            return Json(product);
        }
        ///Vendor bills post
        [HttpGet]
        public IActionResult DraftBills(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchaseOrder = _context.PurchaseOrders.Include(c=>c.Vendor).Where(c => c.Id == id).FirstOrDefault();
            var purchaseOrderItems = _context.PurchaseOrderLineItems.Include(c=>c.Product).Where(c => c.PurchaseOrderId == id).ToList();
            DraftBillsVm draft = new DraftBillsVm();
            draft.Id = purchaseOrder.Id;
            draft.DisplayName = purchaseOrder.Vendor.DisplayName;
            draft.POrderNo = purchaseOrder.PurchaseNo;
            draft.OrderDate = purchaseOrder.OrderDate.ToString();
            draft.ShippingCost = purchaseOrder.ShippingCost;
            draft.Discount = purchaseOrder.Discont;
            draft.OrderTotal = purchaseOrder.TotalAmount;
            draft.Address = purchaseOrder.Vendor.Address;
            draft.Phone = purchaseOrder.Vendor.Phone;
            draft.GrossTotal = 0;

            DraftBillsLineItemVm prodVm;
            draft.DraftBillsLineItemVms = new List<DraftBillsLineItemVm>();
            foreach (var item in purchaseOrderItems)
            {
                prodVm = new DraftBillsLineItemVm();
                prodVm.ProductName = item.Product.Name;
                prodVm.Price = item.Price.Value;
                prodVm.PerProductCost = item.PerProductCost.Value;
                prodVm.Rate = item.Rate.Value;
                prodVm.Discount =0;
                prodVm.Quantity = item.OrderQuantity.Value;
                prodVm.ProductTotal = item.TotalCost.Value;
                draft.GrossTotal = item.TotalCost.Value + draft.GrossTotal;
                draft.DraftBillsLineItemVms.Add(prodVm);
            }

            return View(draft);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DraftBills(DraftBillsVm draftBillsVm)
        {
            if (ModelState.IsValid)
            {
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "BILL").FirstOrDefault();
                VendorBill vendorBill = new VendorBill();
                vendorBill.VendorName = draftBillsVm.DisplayName;
                vendorBill.Address = draftBillsVm.Address;
                vendorBill.Phone = draftBillsVm.Phone;
                vendorBill.BillNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                serialNo.SeialNo = serialNo.SeialNo + 1;
                vendorBill.BillDate = DateTime.Now;
                vendorBill.PurchaseOrderNo = draftBillsVm.POrderNo;
                vendorBill.DueDate = draftBillsVm.OrderDate;
                vendorBill.DueAmount = draftBillsVm.OrderTotal;
                vendorBill.PaymentMethod = draftBillsVm.PaymentMethod;
                vendorBill.TotalAmount = draftBillsVm.OrderTotal;
                vendorBill.ShippingCost = draftBillsVm.ShippingCost;
                vendorBill.PaymentStatus = "NoPaid";
                vendorBill.BillStatus = "Pending";
                vendorBill.PurchaseOrderId = draftBillsVm.Id;
                _context.VendorBills.Add(vendorBill);

                VendorBillLineItem vendorBillLineItem;
                foreach (var item in draftBillsVm.DraftBillsLineItemVms)
                {
                    vendorBillLineItem = new VendorBillLineItem();
                    vendorBillLineItem.VendorBillId = vendorBill.Id;
                    vendorBillLineItem.ProductName = item.ProductName;
                    vendorBillLineItem.ProductPrice = item.Price;
                    vendorBillLineItem.Quantity = item.Quantity;
                    vendorBillLineItem.Discount = item.Discount;
                    vendorBillLineItem.Rate = item.Rate;
                    vendorBillLineItem.ProductTotal = item.ProductTotal;
                    _context.VendorBillLineItems.Add(vendorBillLineItem);

                }
                var purchaseOrder = _context.PurchaseOrders.Where(c => c.Id == draftBillsVm.Id).FirstOrDefault();
                purchaseOrder.PurchaseOrderStatus = "Bill Created";
                _context.Update(purchaseOrder);
              await  _context.SaveChangesAsync();
            }

            const int pageSize = 10;
            int pg = 1;
            if (pg < 1)
            {
                pg = 1;
            }
            var invoices = _context.PurchaseOrders.Include(d => d.Vendor).ToList();
            var resCount = invoices.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            ViewBag.Pager = pager;
            var data = invoices.Skip(resSkip).Take(pager.PageSize);
            //return RedirectToAction("InvoiceIndex", data);
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllPurchaseOrders", data) });

        }
    }

}
