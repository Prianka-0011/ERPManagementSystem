using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ERPManagementSystem.Data;
using AspNetCore.Reporting;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using ERPManagementSystem.Models;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly ApplicationDbContext _context;
        public ReportController(IWebHostEnvironment hosting, ApplicationDbContext context)
        {
            _hosting = hosting;
            _context = context;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public async Task<IActionResult> Index()
        {

            return View();
        }
        

      //public IActionResult PurchaseOrderList()
      //  {
          
      //      var orderList=  _context.PurchaseOrders.ToList();
      //     // PurchaseOrferListReport rpt = new PurchaseOrferListReport(_hosting);
      //      return File());
      //  }

    }
}
