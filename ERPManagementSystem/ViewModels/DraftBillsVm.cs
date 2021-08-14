using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class DraftBillsVm
    {
        public Guid Id { get; set; }
        public string POrderNo { get; set; }
        public string OrderDate { get; set; }
        public string DisplayName { get; set; }
        public string OrderNote { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string PaymentMethod { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Currency { get; set; }
     
        public List<DraftBillsLineItemVm> DraftBillsLineItemVms { get; set; }
    }
}
