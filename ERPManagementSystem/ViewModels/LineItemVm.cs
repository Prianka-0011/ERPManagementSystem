using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class LineItemVm
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Rate { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? PreviousPrice { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal? PerProductCost { get; set; }
        public int? OrderQuantity { get; set; }
        public int? ReceiveQuantity { get; set; }
        public int? DueQuantity { get; set; }
        public decimal? TotalCost { get; set; }
        public string ShortDescription { get; set; }
        public string LargeDescription { get; set; }
        public string ImgPath { get; set; }
        public string ItemStatus { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? TaxRateId { get; set; }
        public Guid? QuotationId { get; set; }

    }
}
