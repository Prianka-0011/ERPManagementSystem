﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class PurchaseOrder
    {
        public  Guid Id { get; set; }
        public string PurchaseNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal ShippingCost { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public decimal Discont { get; set; }
        public decimal TotalAmount { get; set; }
        //Navigation
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public List<PurchaseOrderLineItem> PurchaseOrderLineItems { get; set; }

    }
}