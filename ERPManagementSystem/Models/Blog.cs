using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string ImagePath { get; set; }
        public string BlogHeader { get; set; }
        public string BlogDetails { get; set; }
        public string BlockQuote { get; set; }
        public string BlockQuoteDetails { get; set; }
        public string BlogStatus { get; set; }
    }
}
