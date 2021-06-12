using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class PurchaseOrders
    {
        public PurchaseOrders()
        {
            PurchaseOrderLineItems = new HashSet<PurchaseOrderLineItems>();
        }

        public Guid Id { get; set; }
        public string PurchaseNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal ShippingCost { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public decimal Discont { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid VendorId { get; set; }

        public virtual Vendors Vendor { get; set; }
        public virtual ICollection<PurchaseOrderLineItems> PurchaseOrderLineItems { get; set; }
    }
}
