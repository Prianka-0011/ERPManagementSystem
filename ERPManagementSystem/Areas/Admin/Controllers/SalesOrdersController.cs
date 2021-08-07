using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
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
            if (id==null)
            {
                return NotFound();
            }
            var salesOrder = _context.SaleOrders.Where(c => c.Id==id).FirstOrDefault();
          var saleOrderItems = _context.SaleOrderItems.Where(c => c.SaleOrderId == id).ToList();
            DraftInvoiceVm draft = new DraftInvoiceVm();
            draft.Id = salesOrder.Id;
            draft.DisplayName = salesOrder.DisplayName;
            draft.OrderNo = salesOrder.OrderNo;
            draft.OrderDate = salesOrder.OrderDate.ToString();
            draft.ShippingCost = salesOrder.ShippingCost;
            draft.OrderTotal = salesOrder.OrderTotal;
            draft.GrossTotal = 0;
            draft.PaymentMethod = salesOrder.PaymentMethod;
            StockProductVm prodVm;
            draft.ProductVms = new List<StockProductVm>();
            foreach (var item in saleOrderItems)
            {
                prodVm = new StockProductVm();
                prodVm.ProductName = item.ProductName;
                prodVm.PerProductCost = item.Price;
                prodVm.TaxRate = item.TaxRate;
                prodVm.Discount = item.Discount;
                prodVm.Quantity = item.Quantity;
                prodVm.ProductTotal = item.ProductTotal;
                draft.GrossTotal = item.ProductTotal + draft.GrossTotal;
                draft.ProductVms.Add(prodVm);
            }

            return View(draft);
        }
    }

}
