using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class VendorBill
    {
       
            public Guid Id { get; set; }
            public string BillNo { get; set; }
            public string PurchaseOrderNo { get; set; }
            public DateTime BillDate { get; set; }
            public string DueDate { get; set; }

            public string VendorName { get; set; }
            public string OrderNote { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string PaymentMethod { get; set; }
            public string PaymentStatus { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal DueAmount { get; set; }
            public decimal GivenAmount { get; set; }

            public decimal ShippingCost { get; set; }
            public string Address { get; set; }
            public string BillStatus { get; set; }
            public Guid PurchaseOrderId { get; set; }
            public PurchaseOrder PurchaseOrder { get; set; }
            public List<VendorBillLineItem> VendorBillLineItems { get; set; }


        }
    }
