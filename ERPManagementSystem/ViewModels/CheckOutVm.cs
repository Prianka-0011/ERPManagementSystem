using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class CheckOutVm
    {
        public Guid Id { get; set; }
        public string OderNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string OrderNote { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool CreateAccount { get; set; }
        public string Password { get; set; }
        public string PaymentMethod { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public string Address { get; set; }
        public Guid CountryId { get; set; }
        
        public Guid StateId { get; set; }
       
        public Guid CityId { get; set; }
        public List<StockProductVm> productVms { get; set; }
    }
}
