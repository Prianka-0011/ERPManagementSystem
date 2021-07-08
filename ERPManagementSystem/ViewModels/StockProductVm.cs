using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class StockProductVm
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

        public decimal SalePrice { get; set; }

        public decimal PerProductCost { get; set; }
        public int StockProduct { get; set; }
        public decimal PreviousPrice { get; set; }
        public string ProductSerial { get; set; }
        public decimal Discount { get; set; }
        public decimal ProductTotal { get; set; }
        public int Quantity { get; set; }
        public int CartQuantity { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public string StockProductStatus { get; set; }
        public string ProductName { get; set; }
        public string CurrencyName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }

    }
}
