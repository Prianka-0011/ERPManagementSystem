using ERPManagementSystem.Data;
using ERPManagementSystem.Extensions;
using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ERPManagementSystem.Extensions.Helper;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BlogsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int pg, string sortOrder, string searchString)
        {
            ViewBag.blognam = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";
            var blog = _context.Blogs.Where(c => c.BlogStatus == "Enable");
            switch (sortOrder)
            {
                case "prod_desc":
                    blog = blog.OrderByDescending(n => n.Name);
                    break;
                default:
                    blog = blog.OrderBy(n => n.Name);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                blog = _context.Blogs.Where(c => c.BlogStatus == "Enable" && c.Name.ToLower().Contains(searchString.ToLower()));
            }
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            var resCount = blog.Count();
            ViewBag.TotalRecord = resCount;
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = blog.Skip(resSkip).Take(pager.PageSize);
            ViewBag.Pager = pager;
            return View(await data.ToListAsync());
        }

        //AddOrEdit
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {

                return View(new Blog());
            }

            else
            {
                var blog = await _context.Blogs.FindAsync(id);
                if (blog == null)
                {
                    return NotFound();
                }

                return View(blog);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id, Blog blog, IFormFile ImagePath)
        {
            if (ModelState.IsValid)
            {
                Blog entity;
                string uniqueFileNAme = null;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadimages");
                if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    entity = new Blog();
                    entity.Id = Guid.NewGuid();
                    entity.Name = blog.Name;
                    entity.Date = blog.Date;
                    entity.BlogHeader = blog.BlogHeader;
                    entity.BlogDetails = blog.BlogDetails;
                    entity.BlockQuote = blog.BlockQuote;
                    entity.BlockQuoteDetails = blog.BlockQuoteDetails;
                    entity.BlogStatus = blog.BlogStatus;

                    if (ImagePath != null)
                    {
                        uniqueFileNAme = Guid.NewGuid().ToString() + "_" + ImagePath.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNAme);
                        await ImagePath.CopyToAsync(new FileStream(filePath, FileMode.Create));
                        entity.ImagePath = "uploadimages/" + uniqueFileNAme;
                       
                    }
                    else
                    {
                        entity.ImagePath = "uploadimages/noimage.jpg";
                    }
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    try
                    {
                        entity = await _context.Blogs.FindAsync(blog.Id);
                        entity.Name = blog.Name;
                        entity.Date = blog.Date;
                        entity.BlogHeader = blog.BlogHeader;
                        entity.BlogDetails = blog.BlogDetails;
                        entity.BlockQuote = blog.BlockQuote;
                        entity.BlockQuoteDetails = blog.BlockQuoteDetails;
                        entity.BlogStatus = blog.BlogStatus;
                        if (ImagePath != null)
                        {
                            uniqueFileNAme = Guid.NewGuid().ToString() + "_" + ImagePath.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileNAme);
                            await ImagePath.CopyToAsync(new FileStream(filePath, FileMode.Create));
                            entity.ImagePath = "uploadimages/" + uniqueFileNAme;
                        }

                        _context.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                    }
                }
                var blogData = _context.Blogs.Where(c => c.BlogStatus == "Enable");
                int pg = 1;
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                var resCount = blogData.Count();
                ViewBag.TotalRecord = resCount;
                var pager = new Pager(resCount, pg, pageSize);
                int resSkip = (pg - 1) * pageSize;
                var data = blogData.Skip(resSkip).Take(pager.PageSize);
                ViewBag.Pager = pager;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllBlog", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", blog) });

        }
        //delete category
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FindAsync(id);
            blog.BlogStatus = "Disable";
            await _context.SaveChangesAsync();

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllBlog", _context.Blogs.Where(c => c.BlogStatus == "Enable").ToList()) });

        }
    }
}
