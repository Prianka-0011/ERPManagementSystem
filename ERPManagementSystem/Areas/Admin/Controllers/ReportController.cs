using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ERPManagementSystem.Data;
using AspNetCore.Reporting;
using System.Data;
using Microsoft.EntityFrameworkCore;

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
        //List Report
        [HttpPost]
        public IActionResult PurchaseOrderListReport(DateTime fromDate, DateTime toDate)
        {
            var dt = new DataTable();
            dt = GetPurchaseOrderList(fromDate, toDate);
            //var purchaseOrderList = _context.PurchaseOrders.ToList();
            string mineType = "";
            int extension = 1;
            var path = $"{_hosting.WebRootPath}\\Reports\\PurchaseOrderList.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm", "RDCL Report");
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsPo", dt);
            var result = localReport.Execute(RenderType.Pdf, extension);

            return File(result.MainStream, "application/pdf");
        }
        public DataTable GetPurchaseOrderList(DateTime fromDate, DateTime toDate)
        {

            var purchaseOrder = _context.PurchaseOrders.Include(c=>c.Vendor).Include(c=>c.Currency).ToList();
            if (fromDate != null && toDate != null)
            {
                purchaseOrder = purchaseOrder.Where(c => c.OrderDate >= fromDate && c.OrderDate <= toDate).ToList();
            }
            var dt = new DataTable();
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("PurchaseNo");
            //dt.Columns.Add("DeliveryDate");
            //dt.Columns.Add("Discont");
            //dt.Columns.Add("ShippingCost");
            
            //dt.Columns.Add("TotalAmount");
            //dt.Columns.Add("Currency");
            //dt.Columns.Add("Vendor");
            DataRow row;
            int j = 1;
            foreach (var item in purchaseOrder)
            {
                row = dt.NewRow();
                row["SerialNo"] = j;
                row["PurchaseNo"] = item.PurchaseNo;
                //row["DeliveryDate"] = item.PurchaseNo;
                //row["Discont"] = item.PurchaseNo;
                //row["ShippingCost"] = item.PurchaseNo;
                //row["TotalAmount"] = item.PurchaseNo;
                //row["CurrencyName"] = item.Currency.CurrencyName;
                //row["DisplayName"] = item.Vendor.DisplayName;

                dt.Rows.Add(row);
                j++;
            }
            return dt;
        }
        [HttpPost]
        public IActionResult PurchaseOrderMDReport(string searchPurchaseOrder)
        {

            //if (searchPurchaseOrder!=null)
            //{
            var dt = new DataTable();
            dt = GetPurchaseOrderDetail(searchPurchaseOrder);
            ViewBag.srcString = searchPurchaseOrder;
            string mineType = "";
            int extension = 1;
            var path = $"{_hosting.WebRootPath}\\Reports\\PurchaseOrderMasterDetail.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("prm", "RDCL Report");
            LocalReport localReport = new LocalReport(path);
           // localReport.AddDataSource("dsPurchaseOrderMD", dt);
            var result = localReport.Execute(RenderType.Pdf, extension);
            return File(result.MainStream, "application/pdf");
            //}




        }
        public DataTable GetPurchaseOrderDetail(string searchPurchaseOrder)
        {


            var purchaseOrder = _context.PurchaseOrders.Include(c=>c.Vendor).Where(c => c.PurchaseNo == searchPurchaseOrder).FirstOrDefault();
            if (purchaseOrder != null)
            {
                var dt = new DataTable();
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("PurchaseNo");
            dt.Columns.Add("DisplayName");
            dt.Columns.Add("DeliveryDate");
            dt.Columns.Add("OrderDate");
            dt.Columns.Add("Discont");
            dt.Columns.Add("ShippingCost");
            dt.Columns.Add("Name");
            dt.Columns.Add("Price");
            dt.Columns.Add("Discount");
            dt.Columns.Add("TotalCost");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Address");
            dt.Columns.Add("Phone");
            dt.Columns.Add("OrderQuantity");
            dt.Columns.Add("TotalAmount");
            //dt.Columns.Add("TotalAmount");
            DataRow row;
            int j = 1;
           

           
            var lineItem = _context.PurchaseOrderLineItems.Include(c=>c.Product).Where(c => c.PurchaseOrderId == purchaseOrder.Id).ToList();
            foreach (var item in lineItem)
            {
                row = dt.NewRow();
                row["SerialNo"] = j;
                row["Name"] = item.Product.Name;
                row["Price"] = item.Price;
                row["TotalCost"] = item.TotalCost;
                row["Rate"] = item.Rate;
                row["Discount"] = item.Discount;
                row["OrderQuantity"] = item.OrderQuantity;
                row["PurchaseNo"] = purchaseOrder.PurchaseNo;
                row["ShippingCost"] = purchaseOrder.ShippingCost;
                row["DeliveryDate"] = purchaseOrder.DeliveryDate;
                row["OrderDate"] = purchaseOrder.OrderDate;
                row["Discont"] = purchaseOrder.Discont;
                row["DisplayName"] = purchaseOrder.Vendor.DisplayName;
                row["Address"] = purchaseOrder.Vendor.Address;
                row["Phone"] = purchaseOrder.Vendor.Phone;
                row["TotalAmount"] = purchaseOrder.TotalAmount;
                row["Discont"] = purchaseOrder.Discont;
                
                dt.Rows.Add(row);
                j++;
            }

            return dt;
            }
            return new DataTable();
        }

    }
}
