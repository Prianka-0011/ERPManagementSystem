using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Brands
    {
        public Brands()
        {
            Products = new HashSet<Products>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BrandStatus { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
