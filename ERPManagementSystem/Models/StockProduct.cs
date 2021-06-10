using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class StockProduct
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

        public decimal SalePrice { get; set; }

        public decimal PerProductCost { get; set; }
        public decimal Discount { get; set; }

        public int Quantity { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public string Status { get; set; }
        //Navigation
        public Guid PurchaseOrderLineItemId { get; set; }
        public PurchaseOrderLineItem PurchaseOrderLineItem { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
 
    }
}
