using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class SaleOrderItem
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ProductSerial { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotal { get; set; }
        public string SaleItemStatus { get; set; }
        public Guid SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }

    }
}
