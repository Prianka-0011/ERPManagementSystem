using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CashController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CashController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.srcString = searchString;
            ViewBag.amount = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var cash = _context.Cashes.AsQueryable();
            switch (sortOrder)
            {
                case "prod_desc":
                    cash = cash.OrderByDescending(n => n.TransitionNo);
                    break;
                default:
                    cash = cash.OrderBy(n => n.TransitionNo);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                cash = _context.Cashes.Where(c =>  c.TransitionNo.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = cash.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = cash.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }
        [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                Cash cash = new Cash();
                var serialNo = _context.AutoGenerateSerialNumbers.Where(c => c.ModuleName == "CA").FirstOrDefault();
                if (serialNo == null)
                {
                    cash.TransitionNo = "N/A";

                }
                else
                {
                    serialNo.SeialNo = serialNo.SeialNo + 1;
                    _context.Update(serialNo);
                    _context.SaveChanges();
                    cash.TransitionNo = serialNo.ModuleName + "-000" + serialNo.SeialNo.ToString();
                }

               
               
                
                _context.SaveChanges();
                return View(cash);
            }

            else
            {
                var cash = await _context.Cashes.FindAsync(id);
                if (cash == null)
                {
                    return NotFound();
                }
                return View(cash);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Cash cash)
        {
            if (ModelState.IsValid)
            {
                Cash entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Cash();
                    entity.Id = Guid.NewGuid();
                    entity.TransitionNo = cash.TransitionNo;
                    entity.LastTransitionAmout = cash.LastTransitionAmout;
                    entity.TransitioType = cash.TransitioType;
                    entity.LastTransitionAmout = cash.LastTransitionAmout;
                    var check = _context.Cashes.ToList();
                    if (check.Count==0 && cash.TransitioType== "Addition")
                    {
                        entity.TotalBalance = cash.LastTransitionAmout;
                        _context.Cashes.Add(entity);
                    }
                    else
                    {
                        var maxValue = _context.Cashes.Max(x => x.TotalBalance);

                        if (cash.TransitioType == "Addition")
                        {
                            entity.TotalBalance = maxValue + cash.LastTransitionAmout;
                        }
                        if (cash.TransitioType == "Deduction")
                        {
                            entity.TotalBalance = maxValue - cash.LastTransitionAmout;
                        }
                        _context.Add(entity);
                    }
                    
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Cashes.FindAsync(cash.Id);
                        entity.TransitionNo = cash.TransitionNo;
                        entity.LastTransitionAmout = cash.LastTransitionAmout;
                        entity.TransitioType = cash.TransitioType;
                        entity.LastTransitionAmout = cash.LastTransitionAmout;
                        if (cash.TransitioType == "Addition")
                        {
                            entity.TotalBalance = cash.TotalBalance + cash.LastTransitionAmout;
                        }
                        if (cash.TransitioType == "Deduction")
                        {
                            if (entity.TotalBalance> cash.LastTransitionAmout)
                            {
                                entity.TotalBalance = cash.TotalBalance - cash.LastTransitionAmout;
                            }
                            else
                            {
                                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", cash) });

                            }

                        }
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var cashes = _context.Cashes.ToList();
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = cashes.Count();
                ViewBag.TotalRecord = resCount;
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = cashes.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCash", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", cash) });

        }

    }
}
