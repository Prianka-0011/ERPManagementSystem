using System;
using System.Collections.Generic;
using System.Text;
using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERPManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Inventory table
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Gallery>Galleries { get; set; }
        public virtual DbSet<Quotation>Quotations { get; set; }
        public virtual DbSet<QuotationLineItem>QuotationLineItems { get; set; }
        public virtual DbSet<TaxRate>TaxRates { get; set; }
        //crm table
        public virtual DbSet<Vendor>Vendors { get; set; }
        public virtual DbSet<Customer>Customers { get; set; }
        //hrm table
        public virtual DbSet<Designation> Designations { get; set; }


    }
}
