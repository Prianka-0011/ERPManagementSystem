using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmployeeType { get; set; }
        public decimal Salary { get; set; }
        public decimal ReviewSalary { get; set; }
        public string EmployeeStatus { get; set; }
        //Navigaton
        public string RoleId { get; set; }
        public Guid DesignationId { get; set; }
        public Designation Designation { get; set; }
    }
}
