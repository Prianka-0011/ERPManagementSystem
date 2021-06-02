using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string WebSite { get; set; }
        public string Address { get; set; }
    }
}
