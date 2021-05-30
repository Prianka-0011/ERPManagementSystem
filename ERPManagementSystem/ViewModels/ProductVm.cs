using ERPManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class ProductVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        //Navigation
        public Guid CategoryId { get; set; }
        public Guid SubCategoryId { get; set; }
        public Guid BrandId { get; set; }
        public List<IFormFile> Galleries { get; set; }
        public List<Gallery> GalleryImagesPath { get; set; }
    }
}
