using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class InvoiceItem
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        
        public string ProductSerial { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotal { get; set; }
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}