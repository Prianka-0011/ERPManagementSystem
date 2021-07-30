﻿using System;
using System.Collections.Generic;
using System.Text;
using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERPManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public virtual DbSet<PurchaseOrder>PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderLineItem>PurchaseOrderLineItems { get; set; }
        public virtual DbSet<StockProduct>StockProducts { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        //shop 
        public virtual DbSet<ShippingCharge> ShippingCharges { get; set; }
        public virtual DbSet<InvoiceModel> InvoiceModels { get; set; }
        public virtual DbSet<InvoiceItemModel> InvoiceItemModels { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<AutoGenerateSerialNumber> AutoGenerateSerialNumbers { get; set; }
        public virtual DbSet<SaleOrder> SaleOrders { get; set; }
        public virtual DbSet<SaleOrderItem> SaleOrderItems { get; set; }
        public virtual DbSet<Currency> Currencies  { get; set; }
        //crm table
        public virtual DbSet<Vendor>Vendors { get; set; }
        public virtual DbSet<Customer>Customers { get; set; }
        //hrm table
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }


    }
}
