using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class VendorBillsItem
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Rate { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? PreviousPrice { get; set; }

        public decimal? PerProductCost { get; set; }
        public int? OrderQuantity { get; set; }
        public int? ReceiveQuantity { get; set; }
        public int? DueQuantity { get; set; }
        public decimal? TotalCost { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string ImgPath { get; set; }
        public string ItemStatus { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? TaxRateId { get; set; }
        public Guid? QuotationId { get; set; }
        public Guid? InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Quotation Quotation { get; set; }
        public virtual TaxRate TaxRate { get; set; }
    }
}
