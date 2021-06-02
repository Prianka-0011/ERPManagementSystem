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
        public string QuotationNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        public decimal ShippingCost { get; set; }
        public string Status { get; set; }
        //Navigation
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public List<QuotationLineItem> QuotationLineItems { get; set; }
    }
}
