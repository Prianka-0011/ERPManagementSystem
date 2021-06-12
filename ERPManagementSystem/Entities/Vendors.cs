using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Vendors
    {
        public Vendors()
        {
            PurchaseOrders = new HashSet<PurchaseOrders>();
            Quotations = new HashSet<Quotations>();
        }

        public Guid Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; }
        public string Address { get; set; }
        public string VendorStatus { get; set; }

        public virtual ICollection<PurchaseOrders> PurchaseOrders { get; set; }
        public virtual ICollection<Quotations> Quotations { get; set; }
    }
}
