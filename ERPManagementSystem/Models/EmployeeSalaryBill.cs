using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class EmployeeSalaryBill
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string SalaryBillStatus { get; set; }
        public string PaymentStatus { get; set; }
        public List<> MyProperty { get; set; }
    }
}
