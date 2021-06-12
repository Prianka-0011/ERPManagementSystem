using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class QuotationLineItems
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Rate { get; set; }
        public decimal PerProductCost { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public Guid ProductId { get; set; }
        public Guid QuotationId { get; set; }
        public Guid TaxRateId { get; set; }

        public virtual Products Product { get; set; }
        public virtual Quotations Quotation { get; set; }
        public virtual TaxRates TaxRate { get; set; }
    }
}
