using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Products
    {
        public Products()
        {
            Galleries = new HashSet<Galleries>();
            PurchaseOrderLineItems = new HashSet<PurchaseOrderLineItems>();
            QuotationLineItems = new HashSet<QuotationLineItems>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string ProductStatus { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubCategoryId { get; set; }
        public Guid BrandId { get; set; }

        public virtual Brands Brand { get; set; }
        public virtual Categories Category { get; set; }
        public virtual SubCategories SubCategory { get; set; }
        public virtual ICollection<Galleries> Galleries { get; set; }
        public virtual ICollection<PurchaseOrderLineItems> PurchaseOrderLineItems { get; set; }
        public virtual ICollection<QuotationLineItems> QuotationLineItems { get; set; }
    }
}
