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
                Quotation quotationVm = new Quotation();
                quotationVm.QuotationLineItems = new List<QuotationLineItem>{ new QuotationLineItem{ Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), ImgPath = "images/noimg.png" } };
                ViewData["Products"] = new SelectList(_context.Products.ToList(), "Id", "Name");
                ViewData["Currency"] = new SelectList(_context.Currencies.ToList(), "Id", "CurrencyName");
                ViewData["Tax"] = new SelectList(_context.TaxRates.ToList(), "Id", "Name");
                ViewData["Vendor"] = new SelectList(_context.Vendors.ToList(), "Id", "DisplayName");
                return View(quotationVm);
            }

            else
            {
                var quotation = await _context.Quotations.FindAsync(id);
                
                if (quotation == null)
                {
                    return NotFound();
                }
                Quotation quotationVm = new Quotation();
                var QuotationLineItems = _context.QuotationLineItems.Where(c => c.QuotationId == quotation.Id).ToList();
                quotationVm.Id = quotation.Id;
                quotationVm.QuotationNo = quotation.QuotationNo;
                quotationVm.VendorId = quotation.VendorId;
                quotationVm.Date = quotation.Date;
                quotationVm.ShippingCost = quotation.ShippingCost;
                quotationVm.CurrencyId = quotation.CurrencyId;
                quotationVm.QuotationLineItems = QuotationLineItems;
                ViewData["Products"] = new SelectList(_context.Products.ToList(), "Id", "Name");
                ViewData["Tax"] = new SelectList(_context.TaxRates.ToList(), "Id", "Name");
                ViewData["Currency"] = new SelectList(_context.Currencies.ToList(), "Id", "CurrencyName");
                ViewData["Vendor"] = new SelectList(_context.Vendors.ToList(), "Id", "DisplayName");
                return View(quotationVm);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Quotation quotationVm)
        {
           
                Quotation entity;

                QuotationLineItem lineItem;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {

                    entity = new Quotation();
                    entity.QuotationNo = quotationVm.QuotationNo;
                    entity.VendorId = quotationVm.VendorId;
                    entity.ShippingCost = quotationVm.ShippingCost;
                    entity.Date = DateTime.Now;
                    entity.QuotatonStatus = quotationVm.QuotatonStatus;
                    entity.CurrencyId = quotationVm.CurrencyId;
                    _context.Quotations.Add(entity);
                  
                    foreach (var item in quotationVm.QuotationLineItems)
                    {
                        lineItem = new QuotationLineItem();
                        lineItem.ProductId = item.ProductId;
                        lineItem.Color = item.Color;
                        lineItem.Size = item.Size;
                        lineItem.Price = item.Price;
                        lineItem.TaxRateId = item.TaxRateId;
                        lineItem.Rate = item.Rate;
                        lineItem.QuotationId = entity.Id;
                        lineItem.PerProductCost = item.PerProductCost;
                        lineItem.Discount = item.Discount;
                        lineItem.Quantity = item.Quantity;
                        lineItem.Description = item.Description;
                        lineItem.ImgPath = item.ImgPath;
                        lineItem.TotalCost = item.TotalCost;
                        _context.QuotationLineItems.Add(lineItem);
                    }

                }

                else
                {
                    try
                    {
                        entity =await _context.Quotations.FindAsync(id);
                        entity.QuotationNo = quotationVm.QuotationNo;
                        entity.VendorId = quotationVm.VendorId;
                        entity.ShippingCost = quotationVm.ShippingCost;
                        entity.QuotatonStatus = quotationVm.QuotatonStatus;
                        entity.CurrencyId = quotationVm.CurrencyId;
                    var oldLineIetm =await _context.QuotationLineItems.Where(c => c.QuotationId == id).ToListAsync();
                        foreach (var item in oldLineIetm)
                        {
                            _context.Remove(item);
                        }
                        foreach (var item in quotationVm.QuotationLineItems)
                        {
                            lineItem = new QuotationLineItem();
                            lineItem.ProductId = item.ProductId;
                            lineItem.Color = item.Color;
                            lineItem.Size = item.Size;
                            lineItem.Price = item.Price;
                            lineItem.TaxRateId = item.TaxRateId;
                            lineItem.TaxRate = item.TaxRate;
                            lineItem.QuotationId = entity.Id;
                            lineItem.PerProductCost = item.PerProductCost;
                            lineItem.Discount = item.Discount;
                            lineItem.Quantity = item.Quantity;
                            lineItem.Description = item.Description;
                            lineItem.ImgPath = item.ImgPath;
                            lineItem.TotalCost = item.TotalCost;
                        _context.QuotationLineItems.Add(lineItem);
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
                var resultQotation = _context.Quotations.Include(c => c.Vendor).ToList();
                var resCount = resultQotation.Count();
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                ViewBag.Pager = pager;
                var data = resultQotation.Skip(resSkip).Take(pager.PageSize);

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllQuotation", data) });
            
           // return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit",quotationVm) });

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
