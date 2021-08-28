using ERPManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class DraftEmpSalaryBillsVm
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string SalaryBillStatus { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalSalaryBill { get; set; }
        public List<DraftEmpSalaryBillsItemVm> EmpSalaryBillsItemVms { get; set; }
    }
}
