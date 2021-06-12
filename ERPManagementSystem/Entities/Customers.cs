using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Customers
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CustomerStatus { get; set; }
    }
}
