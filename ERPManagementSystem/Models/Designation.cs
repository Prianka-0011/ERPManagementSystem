using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Designation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string DesignationStatus { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
