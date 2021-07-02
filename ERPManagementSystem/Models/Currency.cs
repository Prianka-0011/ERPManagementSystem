using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyStatus { get; set; }
    }
}
