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
    public class PurchaseOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg)
        {
            var purchaseOrder = _context.PurchaseOrders.Include(c => c.Vendor);
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = purchaseOrder.Count();
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

                ViewData["Tax"] = new SelectList(_context.TaxRates.ToList(), "Id", "Name");
                ViewData["Vendor"] = new SelectList(_context.Vendors.ToList(), "Id", "DisplayName");
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
                var PurchaseOrderLineItems = _context.PurchaseOrderLineItems.Where(c => c.PurchaseOrderId == purchaseOrder.Id).ToList();
                purchaseOrderVm.Id = purchaseOrder.Id;
                purchaseOrderVm.PurchaseNo = purchaseOrder.PurchaseNo;
                purchaseOrderVm.VendorId = purchaseOrder.VendorId;
                purchaseOrderVm.OrderDate = purchaseOrder.OrderDate;
                purchaseOrderVm.DeliveryDate = purchaseOrder.DeliveryDate;
                purchaseOrderVm.ShippingCost = purchaseOrder.ShippingCost;
                purchaseOrderVm.PurchaseOrderLineItems = PurchaseOrderLineItems;
                ViewData["Products"] = new SelectList(_context.Products.ToList(), "Id", "Name");
                ViewData["Tax"] = new SelectList(_context.TaxRates.ToList(), "Id", "Name");
                ViewData["Vendor"] = new SelectList(_context.Vendors.ToList(), "Id", "DisplayName");
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
                _context.PurchaseOrders.Add(entity);
                await _context.SaveChangesAsync();
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
                    lineItem.Description = item.Description;
                    lineItem.ImgPath = item.ImgPath;
                    lineItem.TotalCost = item.TotalCost;
                    lineItem.ItemStatus = item.ItemStatus;
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
                    var oldLineIetm = await _context.PurchaseOrderLineItems.Where(c => c.QuotationId == id).ToListAsync();
                    foreach (var item in oldLineIetm)
                    {
                        _context.Remove(item);
                    }
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
                        lineItem.Description = item.Description;
                        lineItem.ImgPath = item.ImgPath;
                        lineItem.TotalCost = item.TotalCost;
                        lineItem.ItemStatus = item.ItemStatus;
                        _context.PurchaseOrderLineItems.Add(lineItem);
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
            var resulProduct = _context.PurchaseOrders.Include(c => c.Vendor).ToList();
            var resCount = resulProduct.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            ViewBag.Pager = pager;
            var data = resulProduct.Skip(resSkip).Take(pager.PageSize);

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllPurchaseOrder", data) });

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
    }

}
