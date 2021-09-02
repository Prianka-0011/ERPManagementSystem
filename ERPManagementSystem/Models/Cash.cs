using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Cash
    {
        public Guid Id { get; set; }
        public string TransitionNo { get; set; }
        public string TransitioType { get; set; }
        public string SourchDocNo { get; set; }
        public decimal LastTransitionAmout { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
