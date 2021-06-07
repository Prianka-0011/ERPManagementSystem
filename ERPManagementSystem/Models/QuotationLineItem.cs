using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class QuotationLineItem
    {
        public Guid Id { get; set; }
        public string  Color { get; set; }
        public string  Size { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal PerProductCost { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        //Navigation
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid QuotationId { get; set; }
        public Quotation Quotation { get; set; }
        public Guid TaxRateId { get; set; }
        public TaxRate TaxRate { get; set; }
    }
}
