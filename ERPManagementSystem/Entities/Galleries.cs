using System;
using System.Collections.Generic;

namespace ERPManagementSystem.Entities
{
    public partial class Galleries
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        public Guid ProductId { get; set; }

        public virtual Products Product { get; set; }
    }
}
