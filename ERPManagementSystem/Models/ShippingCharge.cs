using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class ShippingCharge
    {
        public Guid Id { get; set; }
        public decimal  BaseCharge { get; set; }
        public decimal IncreaeChargePerProduct { get; set; }
    }
}
