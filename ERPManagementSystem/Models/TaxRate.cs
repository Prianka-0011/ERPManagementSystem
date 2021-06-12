using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class TaxRate
    {
        public TaxRate()
        {
            PurchaseOrderLineItems = new HashSet<PurchaseOrderLineItem>();
            QuotationLineItems = new HashSet<QuotationLineItem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string TaxRateStatus { get; set; }

        public virtual ICollection<PurchaseOrderLineItem> PurchaseOrderLineItems { get; set; }
        public virtual ICollection<QuotationLineItem> QuotationLineItems { get; set; }
    }
}
