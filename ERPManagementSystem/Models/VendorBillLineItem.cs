using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class VendorBillLineItem
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        public string ProductSerial { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotal { get; set; }
        public Guid VendorBillId { get; set; }
        public VendorBill VendorBill { get; set; }
    }
}
