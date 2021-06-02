using ERPManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class QuotationVm
    {
        public Guid Id { get; set; }
        public string QuotationNo { get; set; }
        public DateTime Date { get; set; }
        public decimal ShippingCost { get; set; }
        public string Status { get; set; }
        //Navigation
        public Guid VendorId { get; set; }
        public List<QuotationLineItem>QuotationLineItems { get; set; }
    }
}
