using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class SaleOrder
    {
        public Guid Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string OrderNote { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public string Address { get; set; }
        public string SaleOrderStatus { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public Guid StateId { get; set; }
        public State State { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        

    }
}
