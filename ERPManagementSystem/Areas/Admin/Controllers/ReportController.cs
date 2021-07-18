using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ERPManagementSystem.Data;
using AspNetCore.Reporting;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly ApplicationDbContext _context;
        public ReportController(IWebHostEnvironment hosting,ApplicationDbContext context)
        {
            _hosting = hosting;
            _context = context;
        }
        public IActionResult PurchaseOrderListReport()
        {
            var purchaseOrderList = _context.PurchaseOrders.ToList();
            string mineType = ""; 
            int extension = 1;
            var path = $"{_hosting.WebRootPath}\\Reports\\PurchaseOrder.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm", "RDCL Report");
            LocalReport localReport = new LocalReport(path);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters);
            return File(result.MainStream,"application/pdf");
        }
    }
}
