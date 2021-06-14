using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Data;
using ERPManagementSystem.Models;
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

        public async Task<IActionResult> ProductDetail(Guid id)
        {
            //  StockProduct product;
            var product = await _context.StockProducts.Include(d => d.Product).Where(s => s.Id == id).FirstOrDefaultAsync();

            ViewBag.gallery = _context.Galleries.Where(c => c.ProductId == product.ProductId);
            return View(product);
        }
    }
}
