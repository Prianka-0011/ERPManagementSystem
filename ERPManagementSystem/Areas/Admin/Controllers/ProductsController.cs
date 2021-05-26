using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPManagementSystem.Data;
using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var product = _context.Products.Where(c => c.Status == "Enable");
            return View(await product.ToListAsync());
        }
        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return View(new Product());
            }

            else
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Product product)
        {
            if (ModelState.IsValid)
            {
                Product entity;
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Product();
                    entity.Id = Guid.NewGuid();
                    entity.Name = product.Name;
                    entity.CategoryId = product.CategoryId;
                    entity.SubCategoryId = product.SubCategoryId;
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Categories.FindAsync(category.Id);
                        entity.Name = category.Name;
                        entity.Status = category.Status;
                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCategory", _context.Categories.Where(c => c.Status == "Enable").ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", category) });

        }
    }
}
