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
            return View(products);
        }
    }
}
