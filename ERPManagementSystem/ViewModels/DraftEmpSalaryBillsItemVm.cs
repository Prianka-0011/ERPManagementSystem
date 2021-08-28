using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class DraftEmpSalaryBillsItemVm
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal TotalSalary { get; set; }
    }
}
