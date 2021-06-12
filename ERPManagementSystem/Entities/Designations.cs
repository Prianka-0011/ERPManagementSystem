using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Designations
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string DesignationStatus { get; set; }
    }
}
