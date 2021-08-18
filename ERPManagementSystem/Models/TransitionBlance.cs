using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class TransitionBlance
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string TransitionNo { get; set; }
        public decimal TransBalance { get; set; }
        public string TransitionType { get; set; }
    }
}
