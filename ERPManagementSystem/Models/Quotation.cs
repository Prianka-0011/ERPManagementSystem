using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Quotation
    {
        public Guid Id { get; set; }
        [Required]
        public string QuotationNo { get; set; }

       
        public DateTime Date { get; set; }
        [Required]
        public decimal ShippingCost { get; set; }
        public string QuotatonStatus { get; set; }
        //Navigation
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public List<QuotationLineItem> QuotationLineItems { get; set; }
        public List<PurchaseOrderLineItem> PurchaseOrderLineItems { get; set; }
    }
}
