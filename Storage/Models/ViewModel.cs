using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class ViewModel
    {
        public List<Product> Products { get; set; }
        public SelectList Category { get; set; }
        public string CategoryId { get; set; }
        public string SearchString { get; set; }
    }
}
