using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERPManagementSystem.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckOutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CheckoutDetail()
        {
            List<StockProductVm> products = HttpContext.Session.Get<List<StockProductVm>>("products");
            ViewData["Countries"] = new SelectList(_context.Countries.ToList(), "Id", "Name");
            decimal subtotal = 0;
            int squantity = 0;
            

            foreach (var item in products)
            {
                subtotal = subtotal + item.ProductTotal;
                squantity = item.CartQuantity + squantity;
            }
            var shippingCharge = _context.ShippingCharges.FirstOrDefault();
            var scharge2 = ((squantity - 1) * shippingCharge.IncreaeChargePerProduct)+ shippingCharge.BaseCharge;
            var finalCharge =scharge2;
            ViewBag.finalCharge = finalCharge;
            ViewBag.subTotal = subtotal+finalCharge;
            CheckOutVm checkOutVm = new CheckOutVm();
            checkOutVm.productVms = products;
            return View(checkOutVm);
        }
    }
}
