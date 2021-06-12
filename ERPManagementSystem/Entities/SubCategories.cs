using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class SubCategories
    {
        public SubCategories()
        {
            Products = new HashSet<Products>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SubCategoryStatus { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
