using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class TaxRates
    {
        public TaxRates()
        {
            PurchaseOrderLineItems = new HashSet<PurchaseOrderLineItems>();
            QuotationLineItems = new HashSet<QuotationLineItems>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string TaxRateStatus { get; set; }

        public virtual ICollection<PurchaseOrderLineItems> PurchaseOrderLineItems { get; set; }
        public virtual ICollection<QuotationLineItems> QuotationLineItems { get; set; }
    }
}
