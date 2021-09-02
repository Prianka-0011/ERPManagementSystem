using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class EmployeeSalaryBillItem
    {
        public Guid Id { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal TotalSalary { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid EmployeeSalaryBillId { get; set; }
        public EmployeeSalaryBill EmployeeSalaryBill { get; set; }
    }
}
