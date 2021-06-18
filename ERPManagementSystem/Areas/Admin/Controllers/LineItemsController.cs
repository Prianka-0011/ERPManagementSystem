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
    public class LineItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LineItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
            var lineItem = _context.PurchaseOrderLineItems.Include(d => d.Product);
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = lineItem.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = lineItem.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }
        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            var entity = await _context.PurchaseOrderLineItems.FindAsync(id);
            LineItemVm lineItem = new LineItemVm();
            lineItem.ReceiveQuantity = entity.DueQuantity;
            lineItem.SalePrice = entity.SalePrice;
            lineItem.PreviousPrice = entity.PreviousPrice;
            lineItem.ShortDescription = entity.ShortDescription;
            if (lineItem == null)
            {
                return NotFound();
            }
            return View(lineItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, LineItemVm lineItem)
        {
            if (ModelState.IsValid)
            {
                PurchaseOrderLineItem entity;
                StockProduct stockProduct;
                try
                {
                    entity = await _context.PurchaseOrderLineItems.FindAsync(lineItem.Id);
                    entity.SalePrice = lineItem.SalePrice;
                    entity.ReceiveQuantity =entity.ReceiveQuantity + lineItem.ReceiveQuantity;
                    entity.DueQuantity = entity.OrderQuantity - entity.ReceiveQuantity;
                    entity.PreviousPrice = lineItem.PreviousPrice;
                    entity.ShortDescription = lineItem.ShortDescription;
                    _context.Update(entity);
                    stockProduct = await _context.StockProducts.Where(c => c.PurchaseOrderLineItemId == entity.Id).FirstOrDefaultAsync();
                    if (stockProduct==null)
                    {
                        stockProduct = new StockProduct();
                        stockProduct.PurchaseOrderLineItemId = entity.Id;
                        stockProduct.ProductId = entity.ProductId.Value;
                        stockProduct.ImgPath = entity.ImgPath;
                        stockProduct.Quantity = stockProduct.Quantity+lineItem.ReceiveQuantity.Value;
                        stockProduct.SalePrice = entity.SalePrice.Value;
                        stockProduct.Description = entity.Description;
                        stockProduct.ShortDescription = lineItem.ShortDescription;
                        stockProduct.PreviousPrice = lineItem.PreviousPrice.Value;
                        stockProduct.Color = entity.Color;
                        stockProduct.Size = entity.Size;
                        stockProduct.Description = lineItem.ShortDescription;
                        _context.Add(stockProduct);
                    }
                    else
                    {

                        stockProduct.PurchaseOrderLineItemId = entity.Id;
                        stockProduct.ProductId = entity.ProductId.Value;
                        stockProduct.ImgPath = entity.ImgPath;
                        stockProduct.Quantity = stockProduct.Quantity + lineItem.ReceiveQuantity.Value;
                        stockProduct.SalePrice = entity.SalePrice.Value;

                        stockProduct.Description = entity.Description;
                        stockProduct.ShortDescription = lineItem.ShortDescription;
                        stockProduct.PreviousPrice = lineItem.PreviousPrice.Value;
                        stockProduct.Color = entity.Color;
                        stockProduct.Size = entity.Size;
                        stockProduct.Description = lineItem.ShortDescription;
                        _context.Update(stockProduct);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {

                }
            
            var brandData = _context.PurchaseOrderLineItems.Include(d => d.Product);
            int pg = 1;
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = brandData.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = brandData.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllLineItem", data) });
        }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", lineItem) });

        }

    }
}



