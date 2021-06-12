using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Quotations
    {
        public Quotations()
        {
            PurchaseOrderLineItems = new HashSet<PurchaseOrderLineItems>();
            QuotationLineItems = new HashSet<QuotationLineItems>();
        }

        public Guid Id { get; set; }
        public string QuotationNo { get; set; }
        public DateTime Date { get; set; }
        public decimal ShippingCost { get; set; }
        public string QuotatonStatus { get; set; }
        public Guid VendorId { get; set; }

        public virtual Vendors Vendor { get; set; }
        public virtual ICollection<PurchaseOrderLineItems> PurchaseOrderLineItems { get; set; }
        public virtual ICollection<QuotationLineItems> QuotationLineItems { get; set; }
    }
}
