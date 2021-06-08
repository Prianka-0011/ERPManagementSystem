using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class TaxRate
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public string Status { get; set; }
    }
}
