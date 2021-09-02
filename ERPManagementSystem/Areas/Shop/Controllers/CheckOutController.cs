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

        [HttpGet]
        public IActionResult CheckoutDetail()
        {
            List<StockProductVm> products = HttpContext.Session.Get<List<StockProductVm>>("products");
            ViewData["Countries"] = new SelectList(_context.Countries.ToList(), "Id", "Name");
            decimal subtotal = 0;
            int squantity = 0;
            if (products==null)
            {
                return View();
            }

            foreach (var item in products)
            {
                subtotal = subtotal + item.ProductTotal;
                squantity = item.CartQuantity + squantity;
            }
            decimal finalCharge = 0;
            decimal scharge2 = 0;
            var shippingCharge = _context.ShippingCharges.FirstOrDefault();
            if (shippingCharge != null)
            {
                scharge2 = ((squantity - 1) * shippingCharge.IncreaeChargePerProduct) + shippingCharge.BaseCharge;
                finalCharge = scharge2;
            }
            ViewBag.shippingCharge = finalCharge;
            ViewBag.subTotal = subtotal+finalCharge;
            CheckOutVm checkOutVm = new CheckOutVm();
            checkOutVm.productVms = products;
            return View(checkOutVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutDetail(CheckOutVm checkOutVm)
        {
            
                SaleOrder order = new SaleOrder();
                SaleOrderItem orderItem;
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "SO").FirstOrDefault();

                order.FirstName = checkOutVm.FirstName;
                order.OrderNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();

                order.LastName = checkOutVm.LastName;
                order.DisplayName = checkOutVm.DisplayName;
                order.Email = checkOutVm.Email;
                order.OrderDate = DateTime.Now;
                order.Phone = checkOutVm.Phone;
                order.PaymentMethod = checkOutVm.PaymentMethod;
                order.OrderTotal = checkOutVm.OrderTotal;
                order.OrderNote = checkOutVm.OrderNote;
                order.CountryId = checkOutVm.CountryId;
                order.StateId = checkOutVm.StateId;
                order.CityId = checkOutVm.CityId;
                order.Address = checkOutVm.Address;
                order.ShippingCost = checkOutVm.ShippingCost;
                order.SaleOrderStatus = "Pending";
                serialNo.SeialNo = serialNo.SeialNo + 1;
                _context.Update(serialNo);
                _context.SaleOrders.Add(order);
                List<StockProductVm> products = HttpContext.Session.Get<List<StockProductVm>>("products");
                foreach (var item in products)
                {
                    orderItem = new SaleOrderItem();
                    orderItem.ProductName = item.ProductName;
                    orderItem.Quantity = item.CartQuantity;
                    orderItem.SaleOrderId = order.Id;
                    orderItem.ProductSerial = item.ProductSerial;
                    orderItem.Price = item.SalePrice;
                    orderItem.ProductTotal = item.ProductTotal;
                    _context.SaleOrderItems.Add(orderItem);
                }
               
                await _context.SaveChangesAsync();
                HttpContext.Session.Set("order", order);


                if (order.PaymentMethod == "Bkash")
                {
                  return  RedirectToAction(nameof(BkashPayment));
                }
                else
                {
                  return  RedirectToAction(nameof(CashOnDeliveryPayment));
                }

    
        }
        [HttpGet]
        public IActionResult BkashPayment()
        {
            SaleOrder order= HttpContext.Session.Get<SaleOrder>("order");
            return View(order);
        }
        [HttpGet]
        public IActionResult CancleOrder(Guid id)
        {
            if (id!=null || id!=Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
               var order= _context.SaleOrders.Find(id);
                _context.SaleOrders.Remove(order);
                var orderItem = _context.SaleOrderItems.Where(c => c.SaleOrderId == id);
                foreach (var item in orderItem)
                {
                    _context.SaleOrderItems.Remove(item);
                }
               
            }
            _context.SaveChanges();
            return RedirectToAction("Cart","Home");
        }
        [HttpGet]
        public IActionResult ConfirmAddress(Guid id)
        {
            if (id != null || id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                var order = _context.SaleOrders.Find(id);
                order.SaleOrderStatus = "Confirmed";
                _context.SaleOrders.Update(order);
                
            }
            _context.SaveChanges();
            SaleOrder orderSession = HttpContext.Session.Get<SaleOrder>("order");
            List<StockProductVm> products = HttpContext.Session.Get<List<StockProductVm>>("products");
            orderSession = new SaleOrder();
            products = new List<StockProductVm>();
            HttpContext.Session.Set("order", orderSession);
            HttpContext.Session.Set("products", products);
            return View( "ThankYou");
        }
        [HttpGet]
        public IActionResult ThankYou()
        {
           
            return View("");
        }
        [HttpGet]
        public IActionResult CashOnDeliveryPayment()
        {
            
            SaleOrder order = HttpContext.Session.Get<SaleOrder>("order");
            List<StockProductVm> products = HttpContext.Session.Get<List<StockProductVm>>("products");
            if (products==null)
            {
                return NoContent();
            }
            ViewBag.Products = products;
            return View(order);
        }
        [HttpGet("/CheckOut/GetAllState")]
        public IActionResult GetAllState(Guid id)
        {
            var subCategory = _context.States.Where(c => c.CountryId == id).ToList();
            return Json(subCategory);
        }
        [HttpGet("/CheckOut/GetAllCity")]
        public IActionResult GetAllCity(Guid id)
        {
            var subCategory = _context.Cities.Where(c => c.StateId == id).ToList();
            return Json(subCategory);
        }
    }
}
