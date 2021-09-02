using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class DraftBillsLineItemVm
    {

        public Guid Id { get; set; }

      
        public decimal TaxRate { get; set; }
        
        public string ProductSerial { get; set; }
        public decimal Discount { get; set; }
        public decimal ProductTotal { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public decimal PerProductCost { get; set; }
        public int Quantity { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public string CurrencyName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
