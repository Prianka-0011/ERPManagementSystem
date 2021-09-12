using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using ERPManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeSalaryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeSalaryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.referen = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var monthlySalary = _context.EmployeeSalaryBills.AsQueryable();
            switch (sortOrder)
            {
                case "prod_desc":
                    monthlySalary = monthlySalary.OrderByDescending(n => n.ReferenceNo);
                    break;
                default:
                    monthlySalary = monthlySalary.OrderBy(n => n.ReferenceNo);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                monthlySalary = _context.EmployeeSalaryBills.Where(c => c.ReferenceNo.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = monthlySalary.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = monthlySalary.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                EmployeeSalaryBill employeeSalaryBill = new EmployeeSalaryBill();
                employeeSalaryBill.EmployeeSalaryBillItems = new List<EmployeeSalaryBillItem> { new EmployeeSalaryBillItem { Id = Guid.Parse("00000000-0000-0000-0000-000000000000") } };
                ViewData["Employee"] = new SelectList(_context.Employees.ToList(), "Id", "FirstName");
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "ES").FirstOrDefault();
                if (serialNo != null)
                {
                    employeeSalaryBill.ReferenceNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                    serialNo.SeialNo = serialNo.SeialNo + 1;
                    _context.AutoGenerateSerialNumbers.Update(serialNo);
                }
                else
                {
                    employeeSalaryBill.ReferenceNo = "N/A";

                }

                _context.SaveChanges();
                return View(employeeSalaryBill);
            }

            else
            {
                var employeeBill = await _context.EmployeeSalaryBills.FindAsync(id);
                if (employeeBill == null)
                {
                    return NotFound();
                }
                ViewData["Employee"] = new SelectList(_context.Employees.ToList(), "Id", "FirstName");

                return View(employeeBill);
            }

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddOrEdit(EmployeeSalaryBill bill)
        //{


        //    var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "CA").FirstOrDefault();

        //    Cash cash = new Cash();
        //    if (serialNo == null)
        //    {
        //        cash.TransitionNo = "N/A";

        //    }
        //    else
        //    {
        //     cash.TransitionNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
        //    _context.AutoGenerateSerialNumbers.Update(serialNo);
        //    _context.SaveChanges();
        //    }

        //    cash.TransitioType = "Deduction";
        //    cash.LastTransitionAmout = bill.TotalSalaryBill;
        //    decimal maxValue = _context.Cashes.Max(x => x.TotalBalance);

        //    if (cash.TransitioType == "Addition")
        //    {
        //        cash.TotalBalance = maxValue + cash.LastTransitionAmout;
        //    }
        //    if (cash.TransitioType == "Deduction")
        //    {
        //        cash.TotalBalance = maxValue - cash.LastTransitionAmout;
        //    }
        //    cash.SourchDocNo = bill.ReferenceNo;

        //    var currentBill = _context.EmployeeSalaryBills.Where(c => c.Id == bill.Id).FirstOrDefault();

        //    currentBill.SalaryBillStatus = "Complete";
        //    currentBill.PaymentStatus = "Paid";

        //    _context.EmployeeSalaryBills.Update(currentBill);
        //    _context.Cashes.Add(cash);
        //    await _context.SaveChangesAsync();
        //    var billList = _context.EmployeeSalaryBills.ToList();
        //    int pg = 1;
        //    const int pageSize = 10;
        //    if (pg < 1)
        //    {
        //        pg = 1;
        //    }
        //    var resCount = billList.Count();
        //    ViewBag.TotalRecord = resCount;
        //    var pager = new Pager(resCount, pg, pageSize);
        //    int resSkip = (pg - 1) * pageSize;
        //    var data = billList.Skip(resSkip).Take(pager.PageSize);
        //    ViewBag.Pager = pager;
        //    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllMonthlySalaryBill", data) });          
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, EmployeeSalaryBill bill)
        {

            EmployeeSalaryBill entity;

            EmployeeSalaryBillItem lineItem;
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                entity = new EmployeeSalaryBill();
                entity.SalaryBillStatus = "Pending";
                entity.PaymentStatus = "NoPaid";
                entity.Date = DateTime.Now;
                entity.TotalSalaryBill = bill.TotalSalaryBill;
                entity.ReferenceNo = bill.ReferenceNo;
                _context.EmployeeSalaryBills.Add(entity);
                foreach (var item in bill.EmployeeSalaryBillItems)
                {
                    lineItem = new EmployeeSalaryBillItem();
                    lineItem.EmployeeId = item.EmployeeId;
                    lineItem.EmployeeSalaryBillId = entity.Id;
                    lineItem.SalaryAmount = item.SalaryAmount;
                    lineItem.BonusAmount = item.BonusAmount;
                    lineItem.TotalSalary = item.TotalSalary;     
                    _context.EmployeeSalaryBillItems.Add(lineItem);
                }
            }
            else
            {
                entity = _context.EmployeeSalaryBills.Where(c => c.Id == id).FirstOrDefault();
               
                entity.TotalSalaryBill = bill.TotalSalaryBill;
                _context.EmployeeSalaryBills.Update(entity);
                foreach (var item in bill.EmployeeSalaryBillItems)
                {
                    lineItem = _context.EmployeeSalaryBillItems.Where(c=>c.EmployeeSalaryBillId==entity.Id).FirstOrDefault();
                    if (lineItem!=null)
                    {
                        lineItem.EmployeeId = item.EmployeeId;
                        lineItem.EmployeeSalaryBillId = entity.Id;
                        lineItem.SalaryAmount = item.SalaryAmount;
                        lineItem.BonusAmount = item.BonusAmount;
                        lineItem.TotalSalary = item.TotalSalary;
                       
                        _context.EmployeeSalaryBillItems.Update(lineItem);

                    }
                    else
                    {
                        lineItem.EmployeeId = item.EmployeeId;
                        lineItem.EmployeeSalaryBillId = entity.Id;
                        lineItem.SalaryAmount = item.SalaryAmount;
                        lineItem.BonusAmount = item.BonusAmount;
                        lineItem.TotalSalary = item.TotalSalary;
                        _context.EmployeeSalaryBillItems.Add(lineItem);

                    }

                }
            }    
  
            
            await _context.SaveChangesAsync();
            var billList = _context.EmployeeSalaryBills.ToList();
            int pg = 1;
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = billList.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = billList.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllMonthlySalaryBill", data) });
        }
        [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> DraftSalaryBills(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var empSalary = _context.EmployeeSalaryBills.Where(c => c.Id == id).FirstOrDefault();
            var empSalaryItem = _context.EmployeeSalaryBillItems.Include(c => c.Employee).Where(c => c.EmployeeSalaryBillId == id).ToList();
            DraftEmpSalaryBillsVm draft = new DraftEmpSalaryBillsVm();
            draft.Id = empSalary.Id;
            draft.Date = empSalary.Date;
            draft.ReferenceNo = empSalary.ReferenceNo;
            draft.TotalSalaryBill = empSalary.TotalSalaryBill;


            DraftEmpSalaryBillsItemVm salaryItemVm;
            draft.EmpSalaryBillsItemVms = new List<DraftEmpSalaryBillsItemVm>();
            foreach (var item in empSalaryItem)
            {
                salaryItemVm = new DraftEmpSalaryBillsItemVm();
                
                salaryItemVm.EmployeeName = item.Employee.FirstName;
                salaryItemVm.SalaryAmount = item.SalaryAmount;
                salaryItemVm.TotalSalary = item.TotalSalary;
                salaryItemVm.BonusAmount = item.BonusAmount;
               
                draft.EmpSalaryBillsItemVms.Add(salaryItemVm);
            }

            return View(draft);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DraftSalaryBills(EmployeeSalaryBill draftBillsVm)
        {
            if (ModelState.IsValid)
            {
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "CA").FirstOrDefault();

                Cash cash = new Cash();
              

                cash.TransitioType = "Deduction";
               
                decimal maxValue = _context.Cashes.Max(x => x.TotalBalance);   
               
                var currentBill = _context.EmployeeSalaryBills.Where(c => c.Id == draftBillsVm.Id).FirstOrDefault();
                if (cash.TransitioType == "Deduction")
                {
                    if (maxValue > cash.LastTransitionAmout)
                    {
                        if (serialNo == null)
                        {
                            cash.TransitionNo = "N/A";

                        }
                        else
                        {
                            cash.TransitionNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                            serialNo.SeialNo = serialNo.SeialNo + 1;
                            _context.AutoGenerateSerialNumbers.Update(serialNo);
                        }
                        
                        _context.AutoGenerateSerialNumbers.Update(serialNo);
                        cash.LastTransitionAmout = draftBillsVm.TotalSalaryBill;
                        cash.TotalBalance = maxValue - cash.LastTransitionAmout;
                        cash.SourchDocNo = currentBill.ReferenceNo;
                        currentBill.SalaryBillStatus = "Complete";
                        currentBill.PaymentStatus = "Paid";
                        _context.Cashes.Add(cash);
                    }

                }
                _context.EmployeeSalaryBills.Update(currentBill);
                await _context.SaveChangesAsync();
            }

            const int pageSize = 10;
            int pg = 1;
            if (pg < 1)
            {
                pg = 1;
            }
            var employeeSalaryBills = _context.EmployeeSalaryBills.ToList();
            var resCount = employeeSalaryBills.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            ViewBag.Pager = pager;
            var data = employeeSalaryBills.Skip(resSkip).Take(pager.PageSize);
            //return RedirectToAction("InvoiceIndex", data);
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllMonthlySalaryBill", data) });

        }
    

    [HttpGet("/EmployeeSalary/GetEmployeeSalary")]
        public IActionResult GetEmployeeSalary(Guid id)
        {
            var product = _context.Employees.FirstOrDefault(x => x.Id == id);
            return Json(product.Salary);
        }

    }
}
