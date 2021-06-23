using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class SaleOrderItem
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int quantity { get; set; }
        public int ProductTotal { get; set; }
        public Guid SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }
    }
}
