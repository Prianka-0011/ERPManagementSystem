using ERPManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Extensions
{
    public class CategorMenu:ViewComponent
    {
        public readonly ApplicationDbContext _Contect;
        public CategorMenu(ApplicationDbContext Contect)
        {
            _Contect = Contect;
        }
       public async Task<IViewComponentResult>InvokeAsync()
        {
            var categories = _Contect.Categories.OrderBy(c => c.Name).ToList();
            return View(categories);
        }
    }
}
