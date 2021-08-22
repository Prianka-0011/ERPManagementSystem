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
        public string SaleOrderNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string DueDate { get; set; }
     
        public string CustomerName { get; set; }
        public string OrderNote { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal ReceiveAmount { get; set; }

        public decimal ShippingCost { get; set; }
        public string Address { get; set; }
        public string InvoiceStatus { get; set; }
        public Guid SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }
        public List<InvoiceLineItem> InvoiceLineItems { get; set; }


    }
}
