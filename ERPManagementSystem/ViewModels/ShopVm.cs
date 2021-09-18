
using ERPManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class ShopVm
    {
        public List<Banner> Banners { get; set; }
        public virtual List<StockProduct> StockProducts { get; set; }
    }
}
