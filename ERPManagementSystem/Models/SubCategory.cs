using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class SubCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        //Navigation
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
