using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERPManagementSystem.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg)
        {
            var product = _context.StockProducts.Include(d => d.Product);

            return View(await product.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            //  StockProduct product;
            var product = await _context.StockProducts.Include(d => d.Product).Where(s => s.Id == id).FirstOrDefaultAsync();
            StockProductVm stockProductVm = new StockProductVm();
            stockProductVm.Id = product.Id;
            stockProductVm.ProductName = product.Product.Name;
            stockProductVm.PreviousPrice = product.PreviousPrice;
            stockProductVm.SalePrice = product.SalePrice;
            stockProductVm.ShortDescription = product.ShortDescription;
            stockProductVm.Description = product.Description;
            stockProductVm.Color = product.Color;
            stockProductVm.Size = product.Size;
            stockProductVm.Quantity = product.Quantity;
            ViewBag.gallery = _context.Galleries.Where(c => c.ProductId == product.ProductId);
            return View(stockProductVm);
        }
        [HttpPost]
        public async Task<IActionResult> ProductDetail(StockProductVm stockProductVm)
        {
            //  StockProduct product;
            List<StockProductVm> products;
            var stockProduct =await _context.StockProducts.Include(c=>c.Product).Where(d=>d.Id==stockProductVm.Id).FirstOrDefaultAsync();
            StockProductVm productVm = new StockProductVm();
            productVm.Id = stockProduct.Id;
            
            productVm.ProductName = stockProduct.Product.Name;
            productVm.ImgPath = stockProduct.Product.ImagePath ;
            productVm.CartQuantity = stockProductVm.CartQuantity;
            productVm.Quantity = stockProduct.Quantity;
            productVm.SalePrice = stockProduct.SalePrice;
            productVm.StockProduct = stockProduct.Quantity;
            productVm.ProductTotal = stockProductVm.CartQuantity * stockProduct.SalePrice;
            //Start Session
            products = HttpContext.Session.Get<List<StockProductVm>>("products");
            if (products == null)
            {
                products = new List<StockProductVm>();
            }
            products.Add(productVm);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }
        //Remove fom cart
        [HttpPost]
        public ActionResult RemoveToCart(StockProductVm stockProductVm)
        {
            List<StockProductVm> products = products = HttpContext.Session.Get<List<StockProductVm>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == stockProductVm.Id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Cart(Guid id)
        {
            List<StockProductVm> products = HttpContext.Session.Get<List<StockProductVm>>("products");
            if (products == null)
            {
                products = new List<StockProductVm>();
                ViewBag.subTotal = 0.00;
            }
            decimal subtotal=0;
            foreach (var item in products)
            {
                subtotal = subtotal + item.ProductTotal;
            }
            ViewBag.subTotal = subtotal;
            return View(products);
           
        }
    }
}
