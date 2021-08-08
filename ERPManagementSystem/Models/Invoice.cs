using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
     
        public string CustomerName { get; set; }
        public string OrderNote { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DueAmount { get; set; }

        public decimal ShippingCost { get; set; }
        public string Address { get; set; }
        public string SaleOrderStatus { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }


    }
}
