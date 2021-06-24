using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CountryStatus { get; set; }
    }
}
