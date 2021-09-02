using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.wwwroot.Reports
{

    public class PurchaseOrferListReport
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly ApplicationDbContext _context;
        public ReportController(IWebHostEnvironment hosting, ApplicationDbContext context)
        {
            _hosting = hosting;
            _context = context;
        }
        #region Declaration
        int _maxColumn = 3;
        Document _document;
        #endregion
        Font font;
        PdfPTable _pdfPTable = new PdfPTable(3);
        MemoryStream _memory = new MemoryStream();
        List<PurchaseOrder> _purchaseOrders = new List<PurchaseOrder>();
        public byte[]Report(List<PurchaseOrder>purchaseOrders)
        {
            _purchaseOrders = purchaseOrders;
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(5f, 5f, 20f, 5f);
            _pdfPTable.HorizontalAlignMent = Element.Align_Left;
            PdfWriter pdfWriter = PdfWriter.GetInstance(_document,_memory);
            _document.Open();
            float[] size = new float[_maxColumn];
        }
    }
}
