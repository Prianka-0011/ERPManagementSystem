using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Categories
    {
        public Categories()
        {
            Brands = new HashSet<Brands>();
            Products = new HashSet<Products>();
            SubCategories = new HashSet<SubCategories>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CategoryStatus { get; set; }

        public virtual ICollection<Brands> Brands { get; set; }
        public virtual ICollection<Products> Products { get; set; }
        public virtual ICollection<SubCategories> SubCategories { get; set; }
    }
}
